## Purpose

Desktop application created for automated sending of email template messages from numbers of mail accounts to contact list imported from a text file.

## Features

1. Sending SMTP messages to the recipient list.
2. Ability to add attachments.
3. Possible to load unlimited number of addresses form a file, adding one by one, sorting, filtering, deleting, exporting address list.
4. Editable address status indicating inactive or to be sent.
5. Ability to configure or delete multiple SMTP accounts from which messages will be sent.
6. Saving limit settings, address list, configured SMTP accounts to XML files.
7. Posible to configure daily, hourly limits for sending messages for each configured mailbox.

## Technology

1. Approaches:
  - simplified MVVM,
  - active filtering list with CollectionViewSource,
  - event handlers,
  - view converter,
  - simple dialog manager.
  
2. Application is using:
  - C#, WPF
