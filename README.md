# Introduction

_RFID Gin Data Management_ is an application that runs on a Windows 10 computer at the gin office.   This application is capable of aggregating data from multiple input sources including:

- Modules scanned in the field using _RFID Module Scan_.
- John Deere Harvest ID Files (downloaded using the Cotton Harvest Data Utility or other software.)
- Data collected on module trucks running the _RFID Truck Scan_ software.

**The**  **RFID Gin Data Management**  **software allows you to:**

- Manage master client, farm, and field lists.
- Manage master truck and driver lists.
- Create pickup lists and dispatch them to module trucks running the _RFID Truck Scan_ software.
- Receive data from module trucks equipped with the _RFID Truck Scan_ software and hardware.
- Track location history and status of modules from the field all the way to the gin feeder.
- Import module data files from an IMAP capable email account.
- Import data files on a schedule from a hard drive folder location.
- See a summary of modules and loads remaining in the field.
- Generate report files in CSV, HID, and PDF format.
- Plot module locations on a map.

**The**  **RFID Truck Scan**  **software performs the following functions:**

- Receives client, farm, field, and pickup lists posted to a _Microsoft Azure Cosmos_ database hosted in the cloud.
- Aggregates data from GPS, RFID tags, and a shaft sensor to track the exact location where modules are loaded and unloaded.
- Automatically detects when modules are on the gin yard or have been dropped at the gin feeder using configured GPS boundary coordinates.
- Displays module locations on an in-cab display.
- Displays driving directions to the field using the Microsoft Maps application built into Windows 10.

# Pre-requisites

### RFID Gin Data Management Software

The _RFID Gin Data Management_ software has the following system pre-requisites:

- Windows 10 operating system
- Reliable internet connection

### RFID Truck Scan Software

The _RFID Truck Scan_ software has the following hardware and software pre-requisites which may require purchasing and/or licensing from their respective vendors:

- Windows 10 operating system
- Impinj Speedway Revolution R420 RFID Reader and antennas
- Ublox EVK-7P GPS receiver connected to PC using a [USB to Serial Port adapter](https://www.bestbuy.com/site/insignia-1-3-usb-to-rs-232-db9-pda-serial-adapter-cable-black/5883029.p?skuId=5883029) with a Prolific chipset.
- Shaft encoder connected to a US Digital Quadrature to USB Adapter ([QSB-D model](https://www.usdigital.com/products/interfaces/pc/usb/QSB))
- Nuvo 5100 VTC automotive PC equipped with 4G cellular modem
- Touch screen display

### Online Services Pre-requisites

To share data between the truck and gin software, a _Microsoft Azure Cosmos_ database subscription is required.    You must also obtain a Google Maps API key so that the system can plot module locations.
