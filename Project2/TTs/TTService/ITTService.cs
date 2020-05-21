using System.Data;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace TTService {
  [ServiceContract]
  public interface ITTService {
        [WebInvoke(Method = "POST", UriTemplate = "/tickets", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        int AddTicket(string author, string problem, string title);

        [WebInvoke(Method = "POST", UriTemplate = "/assign/{ticketId}/{supervisorId}", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        int AssignTicket(string ticketId, string supervisorId);

        [WebInvoke(Method = "POST", UriTemplate = "/update/{ticketId}", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        int updateStatus(string ticketId, int status);

        [WebInvoke(Method = "POST", UriTemplate = "/update/answer/{ticketId}", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        int updateAnswer(string ticketId, string answer);

        [WebGet(UriTemplate="/tickets/{author}", ResponseFormat=WebMessageFormat.Json)]
        [OperationContract]
        DataTable GetTickets(string author);

        [WebGet(UriTemplate="/users", ResponseFormat=WebMessageFormat.Json)]
        [OperationContract]
        DataTable GetUsers();

        [WebGet(UriTemplate = "/supervisors", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        DataTable GetSupervisors();

        [WebGet(UriTemplate = "/specializedSupervisors", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        DataTable GetSpecializedSupervisors();

        [WebGet(UriTemplate = "/tickets", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        DataTable GetAllTickets();

        [WebGet(UriTemplate = "/unassigned/tickets", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        DataTable GetAllUnassignedTickets();
    }
}
