# transportation-api
An API to pull basic information from several transportation systems at Living Sky School Division, including:
 - Versatrans
 - FleetVision

# Endpoints
⚠️ Note that all example data on this page is fictional - driver names and vehicle numbers are fake and for example purposes only.

## Ping
`/ping`` is an API endpoint for validating that the API is running and web requests are able to reach it. It can also be used to test connectivity.

Example output:
```json
{
    "ping": "pong"
}
```

## OpenWorkOrders
The `/OpenWorkOrders` endpoint return a list of all open workorders from _FleetVision_.

Example output for `/OpenWorkOrders/`:
```json
[
  {
    "id": 12345,
    "number": "10000",
    "status": "In Process",
    "workRequested": "Engine trouble",
    "vehicleNumber": "111",
    "created": "2022-04-28T08:03:00",
    "lastUpdated": "2023-09-06T12:19:00",
    "priority": ""
  },
  {
    "id": 12346,
    "number": "10001",
    "status": "Waiting for Parts",
    "workRequested": "Coolant leak",
    "vehicleNumber": "222",
    "created": "2023-02-07T09:18:00",
    "lastUpdated": "2023-08-31T16:51:00",
    "priority": ""
  }
]
```

## Bus Inspections
Our busses must be inspected annually for insurance purposes.

The method in which our bus software tracks these dates is _poor_. As such, this API makes a best effort to find inspections coming due or overdue, but it is likely that some may be missed due to how that data is stored.

We store bus inspection dates and due dates as being attached to the __bus driver__, not the bus itself. As such, if dates are not adjusted when bus drivers are re-assigned, data may be lost.

### BusInspections
The `/BusInspections` endpoint lists all found bus inspections.

Example output for `/BusInspections/`:
```json
[
  {
    "lastCompleted": "2023-12-31T00:00:00",
    "expiration": "2023-12-31T00:00:00",
    "vehicleNumber": "111",
    "driverFirstName": "JOHN",
    "driverLastName": "SMITH"
  },
  {
    "lastCompleted": "2023-01-31T00:00:00",
    "expiration": "2024-01-31T00:00:00",
    "vehicleNumber": "222",
    "driverFirstName": "HARVEY",
    "driverLastName": "BIRDMAN"
  }
]
```


### BusInspections/{year}/{month}

The `/BusInspections/{year}/{month}` endpoint lists all bus inspections that would expire in the given year/month. It does __not__ include overdue inspections.

Example output for `BusInspections/2024/01`:
```json
[
  {
    "lastCompleted": "2023-01-31T00:00:00",
    "expiration": "2024-01-31T00:00:00",
    "vehicleNumber": "111",
    "driverFirstName": "BRUCE",
    "driverLastName": "WAYNE"
  },
  {
    "lastCompleted": "2023-01-31T00:00:00",
    "expiration": "2024-01-31T00:00:00",
    "vehicleNumber": "222",
    "driverFirstName": "CLARK",
    "driverLastName": "KENT"
  }
]
```

### BusInspections/overdue/{year}/{month}

The `/BusInspections/overdue/{year}/{month}` endpoint lists all bus inspections that were missed _prior_ to the given year/month, and which are now overdue. It uses the first day of the given month as a reference day.

Example output for `BusInspections/overdue/2024/01`:
```json
[
  {
    "lastCompleted": "2022-12-31T00:00:00",
    "expiration": "2023-12-31T00:00:00",
    "vehicleNumber": "333",
    "driverFirstName": "PETER",
    "driverLastName": "PARKER"
  }
]
```

