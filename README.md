# Filmtopia
### Cinema Management System — A-Level Computer Science NEA [Eduqas]

A Windows Forms application built in Visual Basic .NET for managing the day-to-day operations of a small independent cinema. Developed as part of the Eduqas A-Level Computer Science Non-Examined Assessment (NEA).

---

## Overview

Filmtopia is a cinema management system designed to replace a paper-based booking process with a fully computerised solution. The system supports three user types — customers, staff and managers — each with their own interface and access level.

The core problem it solves is double booking. By replacing a shared paper diary with a real-time graphical seat selection interface, the system ensures no two customers can be assigned the same seat for the same screening.

---

## Features

- Secure login system with access level control (staff / manager)
- 3-attempt lockout on failed login
- Graphical seat map showing available and booked seats in real time
- Customer self-service kiosk for ticket and food ordering
- Staff booking management — add, search and cancel bookings
- Manager dashboard — add films, schedule screenings, view sales reports
- Microsoft Access database with full relational structure and referential integrity
- Input validation across all forms

---

## Tech Stack

| Layer | Technology |
|---|---|
| Language | Visual Basic .NET |
| Framework | Windows Forms (.NET Framework 4.8) |
| IDE | Visual Studio 2022 |
| Database | Microsoft Access 2003 (.mdb) |
| DB Connection | OleDb (Jet OLEDB 4.0) |

---

## Database Structure

| Table | Purpose |
|---|---|
| tblLogin | User credentials and access levels |
| tblFilm | Film titles, ratings and descriptions |
| tblScreen | Cinema screens and capacities |
| tblScreening | Film screenings linked to screens |
| tblSeat | Individual seats per screen |
| tblCustomer | Customer details |
| tblBooking | Booking records |
| tblBookingSeat | Junction table linking bookings to seats |
| tblFoodItem | Food and drink menu |
| tblOrderItem | Food orders linked to bookings |

---

## Access Levels

| Level | Role | Access |
|---|---|---|
| 1 | Staff | Bookings and screenings |
| 2 | Manager | Full access including films and reports |
| 99 | Customer | Kiosk only |

---

## Project Status

| Version | Status | Notes |
|---|---|---|
| v1.0.0 | Complete | Login, main form, database design |
| v1.1.0 | In progress | Films and screenings management |
| v2.0.0 | Planned | Graphical seat map |
| v3.0.0 | Planned | Customer kiosk |

---

## Author

**Mathew Felix** — Year 12, Holy Cross College, Bury  
Aspiring cybersecurity engineer. Building toward a degree apprenticeship.  
[github.com/mathewfelixx](https://github.com/mathewfelixx)
