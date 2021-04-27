using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; //для Parallel
using System.Collections.Concurrent; //для ConcurrentBag
using System.DirectoryServices; //для DirectoryEntry
using System.Net.NetworkInformation; //для Ping
using System.Management; //для WMI
using CompInfo.Models;
using CompInfoLibrary;
using System.Data.Entity;
using System.IO; //для работы с файлами
using System.Net; //для Dns

namespace CompInfo.Controllers
{
    static class MainController
    {
        public static List<string> GetNamesFromAD()
        {
            List<string> resultList = new List<string>();
            using (DirectoryEntry entry = new DirectoryEntry("LDAP://amgpgu", null, null, AuthenticationTypes.Secure))
            using (DirectorySearcher searcher = new DirectorySearcher(entry))
            {
                string property = "name";
                searcher.Filter = "(&(objectClass=computer))";
                searcher.PageSize = 1000;
                searcher.PropertiesToLoad.Clear();
                searcher.PropertiesToLoad.Add(property);

                var results = searcher.FindAll();
                var searchResults = results.Cast<SearchResult>().ToArray();
                var bag = new ConcurrentBag<string>();

                int desiredPageSize = 2000;
                for (var step = 0; step < Math.Ceiling((double)results.Count / desiredPageSize); step++)
                {
                    Parallel.ForEach(searchResults.Skip(step * desiredPageSize).Take(desiredPageSize), result =>
                    {
                        using (var root = result.GetDirectoryEntry())
                        {
                            root.RefreshCache(new[] { property });
                            if (root.Properties.Contains(property))
                            {
                                bag.Add((string)root.Properties[property][0]);
                            }
                        }
                    });
                }

                StreamWriter objWriter = new StreamWriter(@"name_diapazon.txt", true, Encoding.Default);
                foreach (string comp in bag)
                {
                    resultList.Add(comp);
                    objWriter.WriteLine(comp);
                }
                objWriter.Close();
            }

            return resultList;
        }

        public static string hardwareScan(string compName)
        {
            CompInfoContext db = new CompInfoContext();
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Ping pinger = new Ping();
                    PingReply reply = pinger.Send(compName, 100);
                    if (reply.Status == IPStatus.Success)
                    {
                        string ip = "";
                        IPAddress[] ipaddresses = Dns.GetHostAddresses(compName);
                        foreach (IPAddress addr in ipaddresses)
                        {
                            byte[] b_ipaddress = addr.GetAddressBytes();
                            if ((b_ipaddress[0] == 172) && (b_ipaddress[1] == 16))
                            {
                                ip = addr.ToString();
                            }
                            break;
                        }

                        ConnectionOptions con_options = new ConnectionOptions();
                        con_options.Username = compName + @"\Администратор";
                        con_options.Password = "t[fkuhtrf";
                        ManagementScope scope = new ManagementScope(@"\\" + compName + @"\root\cimv2", con_options);
                        scope.Connect();

                        #region получение информации о компе
                        Computer comp = db.Computers.FirstOrDefault(c => c.Name == compName);
                        if (comp == null) { comp = new Computer(); db.Computers.Add(comp); comp.Name = compName; }

                        comp.IP = ip;

                        //информация об операционной системе
                        ObjectQuery query2 = new ObjectQuery("select Caption, CSDVersion, Version from Win32_OperatingSystem");
                        ManagementObjectSearcher searcher2 = new ManagementObjectSearcher(scope, query2);
                        foreach (ManagementObject sys in searcher2.Get())
                        {
                            comp.OS = sys["Caption"] + " " + sys["Version"] + " " + sys["CSDVersion"];
                        }

                        //информация о текущем пользователе
                        ObjectQuery query1 = new ObjectQuery("select UserName from Win32_ComputerSystem");
                        ManagementObjectSearcher searcher1 = new ManagementObjectSearcher(scope, query1);
                        foreach (ManagementObject username in searcher1.Get())
                        {
                            comp.User = username["UserName"].ToString();
                        }

                        //информация о материнской плате
                        ObjectQuery query4 = new ObjectQuery("select Manufacturer, Product from Win32_BaseBoard");
                        ManagementObjectSearcher searcher4 = new ManagementObjectSearcher(scope, query4);
                        foreach (ManagementObject mother in searcher4.Get())
                        {
                            comp.Motherboard = mother["Manufacturer"] + " " + mother["Product"];
                        }

                        //информация о принтерах
                        ObjectQuery query7 = new ObjectQuery("select Caption, PortName from Win32_Printer where PortName like 'COM%' or PortName like 'LPT%' or PortName like 'USB%'");
                        ManagementObjectSearcher searcher7 = new ManagementObjectSearcher(scope, query7);
                        db.Printers.RemoveRange(db.Printers.Where(p => p.ComputerId == comp.Id));
                        foreach (ManagementObject printer in searcher7.Get())
                        {
                            Printer print = new Printer
                            {
                                Name = printer["Caption"].ToString(),
                                Computer = comp
                            };
                            db.Printers.Add(print);
                        }

                        //информация о жестких дисках
                        ObjectQuery query6 = new ObjectQuery("select Caption, Model, Size from Win32_DiskDrive");
                        ManagementObjectSearcher searcher6 = new ManagementObjectSearcher(scope, query6);
                        db.HardDisks.RemoveRange(db.HardDisks.Where(h => h.ComputerId == comp.Id));
                        foreach (ManagementObject harddisk in searcher6.Get())
                        {
                            HardDisk hard = new HardDisk
                            {
                                Name = harddisk["Caption"].ToString(),
                                Size = (float)Math.Round(Convert.ToDouble(harddisk["Size"]) / 1024 / 1024 / 1024, 0),
                                Computer = comp
                            };
                            db.HardDisks.Add(hard);
                        }

                        //информация об объеме оперативной памяти
                        ObjectQuery query5 = new ObjectQuery("select BankLabel, Capacity, MemoryType, Speed from Win32_PhysicalMemory");
                        ManagementObjectSearcher searcher5 = new ManagementObjectSearcher(scope, query5);
                        db.RAMs.RemoveRange(db.RAMs.Where(r => r.ComputerId == comp.Id));
                        foreach (ManagementObject memory in searcher5.Get())
                        {
                            RAM ram = new RAM();
                            ram.Capacity = (float)Math.Round(Convert.ToDouble(memory["Capacity"]) / 1024 / 1024, 0);
                            if (memory["Speed"] != null) ram.Speed = Convert.ToInt64(memory["Speed"]);
                            ram.Computer = comp;
                            db.RAMs.Add(ram);
                        }

                        //информация о процессоре
                        ObjectQuery query3 = new ObjectQuery("select MaxClockSpeed, Name from Win32_Processor where ProcessorType = 3");
                        ManagementObjectSearcher searcher3 = new ManagementObjectSearcher(scope, query3);
                        db.Processors.RemoveRange(db.Processors.Where(p => p.ComputerId == comp.Id));
                        foreach (ManagementObject cpu in searcher3.Get())
                        {
                            Processor proc = new Processor
                            {
                                Name = cpu["Name"].ToString(),
                                Frequency = (float)Math.Round(Convert.ToDouble(cpu["MaxClockSpeed"]) / 1000, 2),
                                Computer = comp
                            };
                            db.Processors.Add(proc);
                        }
                        #endregion
                    }
                    db.SaveChanges();
                    transaction.Commit();
                    return "Success";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ex.Message;
                }
            }
        }

        public static string GetHardwareFromDB(string compName)
        {
            string result = "";

            CompInfoContext db = new CompInfoContext();
            Computer comp = db.Computers.FirstOrDefault(c => c.Name == compName);
            if (comp != null)
            {
                result += comp.Name + " " + comp.IP + "\n" + comp.User + "\n" + comp.OS + "\n" + comp.Motherboard;
                var procs = db.Processors.Where(p => p.ComputerId == comp.Id);
                foreach (Processor proc in procs) { result += "\n" + "Processor: " + proc.Name + " " + proc.Frequency.ToString() + "GHz"; }
                var rams = db.RAMs.Where(r => r.ComputerId == comp.Id);
                foreach (RAM ram in rams) { result += "\n" + "RAM: " + ram.Capacity.ToString() + "MB " + ram.Speed.ToString(); }
                var hards = db.HardDisks.Where(h => h.ComputerId == comp.Id);
                foreach (HardDisk hard in hards) { result += "\n" + "HardDisk: " + hard.Name + " " + hard.Size.ToString() +"GB"; }
                var prints = db.Printers.Where(p => p.ComputerId == comp.Id);
                foreach (Printer print in prints) { result += "\n" + "Printer: " + print.Name; }
            }
            else { result = "Данных о компьютере нет в базе"; }

            return result;
        }

        public static string PingComp(string compName)
        {
            try
            {
                Ping pinger = new Ping();
                PingReply reply = pinger.Send(compName, 100);
                if (reply.Status == IPStatus.Success)
                { return "Online"; }
                else { return "Offline"; }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }            
        }
    }
}
