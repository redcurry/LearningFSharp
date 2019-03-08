open System.Configuration

[<EntryPoint>]
let main _ =
    let runtimeConnectionString = ConfigurationManager.ConnectionStrings.["AdventureWorks"].ConnectionString
    CustomerRepository.printCustomers(runtimeConnectionString)
    0
