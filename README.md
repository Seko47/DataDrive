# Data-Drive

## Description

It's a simple cloud drive system that allows you to store, share and synchronize your data. The functionality of this web application is based on the functionality of popular cloud drives. 

## Technology stack
* .NET Core 3.0
* Angular 8
* Entity Framework Core 3.0
* xUnit
* Moq

## Functional requirements

### File management

You can upload your own files to the drive. The file you uploaded by your computer can be downloaded by you on another device connected to the Internet. File management includes:
* uploading new files to the drive,
* downloading the uploaded files from the drive,
* deleting the uploaded files when they not needed,
* manipulating the uploaded files, i.e. renaming and moving files between folders.

### Note management

You can create, edit and delete a note at any time. Once created, the note will by available on all your Internet-connected devices. The editor, built in the application, supports simple text formatting.

### Data sharing

The system allows for data sharing in two ways: sharing through a URL link for all and sharing with specific users.

#### Sharing through a URL link

You can share data through a generated, shortened URL link to the resource. Every user who has this address will have access to the shared data, which will allow to read and download the resource. In addition a QR code is generated which will allow you to access the data after scanning. Access to the resource may be limited by providing the expiry date of the link, the limit of the number of downloads (for files) or a password.

#### Sharing with specific users

Sharing with specific users is personalised sharing. No restrictions are needed, because you have to indicate specific users who have an account with the platform. The indicated users will have the possibility to view and download the shared resources.

### Private messages

The system supports sending private messages - chat. To send a message, you have to type in the recipient - the login of the registered account in the system, and the content of the message.

### QR code scanning

The system has a built-in QR code scanner. Every user who has a device with a camera has access to it. After scanning the code, the system will check if the code comes from the application, if so, the user will be automatically redirected to the shared resource.

### System administration

The administrator can change the amount of free space for newly registered users. Additionally, the administrator has the possibility to remove illegal resources that are reported by users.

## Admin account

Login: admin@admin.com

Password: zaq1@WSX
