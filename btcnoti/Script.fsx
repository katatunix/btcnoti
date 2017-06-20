open System.Net

let proxy = new WebProxy("abc.com", 1234);
proxy.Address.ToString ()
