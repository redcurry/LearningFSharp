#r @"packages\FSharp.Data.SqlClient\lib\net40\FSharp.Data.SqlClient.dll"

open FSharp.Data

let [<Literal>] Conn = "Server=(localdb)\MSSQLLocalDB;Database=AdventureWorksLT;Integrated Security=SSPI"
type GetCustomers = SqlCommandProvider<"SELECT * FROM SalesLT.Customer WHERE CompanyName = @CompanyName", Conn>
let customers = GetCustomers.Create(Conn).Execute("A Bike Store") |> Seq.toArray
let customer = customers.[0]

printfn "%d" customers.Length
printfn "%s %s works for %A" customer.FirstName customer.LastName (defaultArg customer.CompanyName "N/A")

type AdventureWorks = SqlProgrammabilityProvider<Conn>
let productCategory = new AdventureWorks.SalesLT.Tables.ProductCategory()

productCategory.AddRow("Mittens", Some 3)
productCategory.AddRow("Long Shorts", Some 3)
productCategory.AddRow("Wooly Hats", Some 4)

productCategory.Update()

type Categories = SqlEnumProvider<"SELECT Name, ProductCategoryId FROM SalesLT.ProductCategory", Conn>
let woolyHats = Categories.``Wooly Hats``
printfn "Wooly Hats has ID %d" woolyHats

#r @"packages\SQLProvider\lib\FSharp.Data.SqlProvider.dll"

open FSharp.Data.Sql

type AdventureWorks2 = SqlDataProvider<ConnectionString = Conn, UseOptionTypes = true>
let context = AdventureWorks2.GetDataContext()
let customers2 =
    query {
        for customer in context.SalesLt.Customer do
        take 10
    } |> Seq.toArray
let customer2 = customers.[0]

let customers3 =
    query {
        for customer in context.SalesLt.Customer do
        where (customer.CompanyName = Some "Shap Bikes")
        select (customer.FirstName, customer.LastName)
        distinct
    }

let mittens =
    context.SalesLt.ProductCategory
        .Individuals.``As Name``.``10, Brakes``