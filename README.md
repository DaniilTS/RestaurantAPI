# RestaurantAPI

Before run change db connection string in appsettings.json!

Changes that has been done:
- RestManager constructor doesn't take tables list as a parameter, because it takes them from database
- ClientsGroup:
  - BoredIndex and MaxBoaredIndex as a parameter that shows at which moment clientsGroup will leave the queue
  - ClientsGroupStatus shows what status can clientsGroup has: InQueue, Boared, AtTable, Served
  - ArrivalTime is used to sort clientsGroup with InQueue status after app restart (if app has been stopped during runtime)
  - LunchTimeInSeconds shows how much time clientsGroup will spend at table
- Delayed tasks are done via Hangfire, and are run in another thread later
