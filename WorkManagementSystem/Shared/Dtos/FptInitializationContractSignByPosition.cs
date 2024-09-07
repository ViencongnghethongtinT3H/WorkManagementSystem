namespace WorkManagementSystem.Shared.Dtos;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Body
{
    public string alias { get; set; }
    public string refId { get; set; }
    public string file { get; set; }
    public string fileName { get; set; }
    public int docTypeCode { get; set; }
    public List<HeaderField> headerFields { get; set; }
    public List<Party> parties { get; set; }
}

public class HeaderField
{
    public string id { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public string value { get; set; }
}

public class Party
{
    public string id { get; set; }
    public bool isMyOrg { get; set; }
    public bool isOrg { get; set; }
    public string orgName { get; set; }
    public int order { get; set; }
    public List<Recipient> recipients { get; set; }
}

public class Recipient
{
    public bool isEsign { get; set; }
    public string recipientId { get; set; }
    public string email { get; set; }
    public string personalName { get; set; }
    public string telephoneNumber { get; set; }
    public string contactId { get; set; }
    public string role { get; set; }
    public int order { get; set; }
    public List<string> notifyTypes { get; set; }
    public List<object> signTypes { get; set; }
}

public class FptInitializationContractSignByPosition
{
    public string id { get; set; }
    public string refId { get; set; }
    public string selector { get; set; }
    public string lookup { get; set; }
    public object attrs { get; set; }
    public string payload { get; set; }
    public Body body { get; set; }
}

public class FptLoginResponse
{
    public string access_token { get; set; }
    public string refresh_token { get; set; }
    public string expTime { get; set; }
}

public class FptLoginRequest
{
    public string clientid { get; set; }
    public string clientsecret { get; set; }
    public string username { get; set; }
    public string password { get; set; }
}

public class Response
{
    public string envelopeId { get; set; }
    public string linkWebView { get; set; }
    public string usernameIndividual { get; set; }
    public string passwordIndividual { get; set; }
    public string statusIndividual { get; set; }
}

public class FptInitializationContractResponse
{
    public string id { get; set; }
    public object refId { get; set; }
    public string code { get; set; }
    public string message { get; set; }
    public object result { get; set; }
    public Response response { get; set; }
}


