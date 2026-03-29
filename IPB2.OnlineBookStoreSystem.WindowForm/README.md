# IPB2.OnlineBookStoreSystem.WindowForm

Windows Forms project for `OnlineBookStoreDB` covering:

- Management Module (CRUD): Authors, Categories, Customers, Books
- Order Module: Create Order, Add Order Item, Update Order, Cancel Order
- Payment Module: Calculate Total, Make Payment, Generate Receipt
- Report Module: Sales Report, Book Stock Report, Customer Order Summary

## Setup

1. Run your SQL script to create `OnlineBookStoreDB`.
2. Open `App.config` and update the `OnlineBookStoreDb` connection string.
3. Build/run the WinForms project from Visual Studio on Windows.

## Notes

- The application uses parameterized SQL via `Microsoft.Data.SqlClient`.
- Orders and stock are updated when items are added/cancelled.
- Payments mark orders as `Paid`.
