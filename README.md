# Ticketing System

There is a [**Testing Report**](https://docs.google.com/document/d/1mXRttkd4_P9TrjN8VnmjGF8kpvxcLOHJ2R621g0iElM/edit?usp=sharing)



## Project Overview

This project introduces a high-performance ticketing system engineered specifically for handling scenarios with high concurrent user activities. Without any hardware scaling, this system is capable of processing the actions of 9,000 users error-free in about half an hour (2200 seconds). Its exceptional load management and resource utilization make it an ideal solution for managing large-scale user requests.



### Key Features

- **User Registration and Login:** Users can register new accounts and log into the system.
- **Browsing Products:** Users can view a list of products and their details.
- **Product Details:** Includes title, description, date, and available tickets.
- **Ticket Purchasing:** Users can select the date and quantity on the product details page to purchase tickets.
- **Sold-Out Handling:** Users are prevented from purchasing if tickets are sold out.



### Project Requirements

- **CSRF Token Authentication:** Ensures secure transactions.
- **Standardized Responses:** All responses must follow a predefined format.
- **Custom Error Codes:** Error handling is managed through specific codes designed for the system.

This documentation provides a straightforward and everyday language overview of the ticketing system project, emphasizing its robustness and efficiency in high-load environments.
