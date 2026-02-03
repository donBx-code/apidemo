
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
namespace apidemo.Models
{

    public class Client
    {
        public int Id {get; set;}
      
        public int client_id {get; set;} 
     
        public int fund_id {get; set;} 
       
        public DateTime as_of_date {get; set;} 
        public string metric_name {get; set;}
        public decimal metric_value  {get; set;}

    }



}
