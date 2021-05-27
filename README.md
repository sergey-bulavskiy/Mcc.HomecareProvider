# Mcc.HomecareProvider

## Overview
This is a oversimplified, buggy version of the application, that was created for HomeCare Provider (further: HCP), to track devices and patients thay have.
You were asked by the HCP to analyze it, play around with API and find as many bugs as you can, and give feedback regarding this application, system design and database structure. Application contains bugs that were made on purpose and bugs that were done accidentally, so there are no specific "number" of bugs that you need to find.

## Domain: 
Domain consists of three entities: Patients, Devices, Device Binding. All entities contains `CreatedAt` field that indicates, when entity was created. 

### Patient: 
Patient is the entity that represents a real Patient that is being treated by HCP staff. Doctors select devices that fit specific patient at the specific time. Patient can have only one device at the time. Patients have personal data that is stored in the database: first name, last name, email, date of birth. Email is unique across the system. Patient contains CurrentBindingId which indicates that patient has device and device is being assigned to him. CurrentBindingId is `null` in case when patient does not have a device.

### Device: 
Device is the entity that represents a real [CPAP](https://en.wikipedia.org/wiki/Positive_airway_pressure) device, which is owned by the HCP. HCP stores devices at the storage, and gives devices to the patients when they need one. Device can be assigned to a patient, and can be not assigned to anyone. Devices serial numbers should fit in range: [100000; 999999]. Serial numbers are unique, there can be no devices with same serial numbers.

### Device Binding:
Device Binding represents the fact that at a specific period of time a device, was assigned to patient, or unassigned from a patient. Device Binding contains Id of the device that binding belongs to. Device should have at least 1 device binding. Device binding can have `PatientId` in case when device was assigned to a patient, null if no patient assigned.

## Database
<details>
  <summary>DB Schema:</summary>

  ![image](https://user-images.githubusercontent.com/1502886/119872502-bda49900-bf4d-11eb-8ac3-f3ca1cc1b43d.png)
  
</details>


## SQL Tasks: 
1. Find patient (specify first/last names) that had the most devices for the whole period of existence.
2. Find what serial number of the device that was assigned to the patient with email "jimi.hendrix@gmail.com" at 7th of May, 2005 year.


## Useful tools: 

### PG Clients:
- free pg db client - https://www.pgadmin.org/
- other db client (paid but with trial) - https://www.jetbrains.com/datagrip/

### API clients: 
- Postman - https://www.postman.com/
- VS Code Rest Client extension - https://marketplace.visualstudio.com/items?itemName=humao.rest-client 
(VS Code rest client extension [snippet](https://github.com/sergey-bulavskiy/Mcc.HomecareProvider/blob/main/Mcc.HomecareProvider.http))
- serviceUrl/swagger/index.html - Third possible way to test out API, should be opened by-default.  (service url will be given to you)
