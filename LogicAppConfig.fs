module TestFarmer1


type Connection =
    { connectionId: string
      connectionName: string
      id: string }

type LogicAppConfiguration =
    { connections: (string * Connection) []
      environment: string
      tenant:string
      clientId:string
      secrete:string
      resourceGroupName: string }

let dynamicsAxUAT =
    { connectionId =
          ""
      connectionName = ""
      id =
          "" }


let serviceBusUAT =
    { connectionId =
          ""
      connectionName = ""
      id =
          "" }



let DynataCustomConnectorUAT =
    { connectionId =
          ""
      connectionName = ""
      id =
          "" }


let destinationLogicAppConfigUAT =
    { connections =
          [| ("dynamicsax", dynamicsAxUAT)
             ("servicebus", serviceBusUAT)
             ("DynataCustomConnector", DynataCustomConnectorUAT) |]
      environment = ""
      clientId = ""
      tenant = ""
      secrete = ""
      resourceGroupName = "" }
    


