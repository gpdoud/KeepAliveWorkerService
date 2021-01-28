using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestWorkerService {

    public class Aping {
        private string url = string.Empty;
        private string tag = string.Empty;

        public async Task<string> Ping() {
            var http = new HttpClient();
            try {
                var response = await http.GetStringAsync(url);
                return $"[{tag}] Ok";
            } catch (Exception ex) {
                return $"[{tag}] ERROR: {ex.Message}";
            }
        }

        public Aping(string url, string tag) {
            this.url = url;
            this.tag = tag;
        }

    }
}
