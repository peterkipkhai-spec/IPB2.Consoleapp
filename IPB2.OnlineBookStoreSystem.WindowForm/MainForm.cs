using System.Data;
using IPB2.OnlineBookStoreSystem.WindowForm.Services;

namespace IPB2.OnlineBookStoreSystem.WindowForm;

public sealed class MainForm : Form
{
    private readonly BookStoreService _service = new();

    private readonly TabControl _tabs = new() { Dock = DockStyle.Fill };

    private readonly DataGridView _dgvAuthors = CreateGrid();
    private readonly DataGridView _dgvCategories = CreateGrid();
    private readonly DataGridView _dgvCustomers = CreateGrid();
    private readonly DataGridView _dgvBooks = CreateGrid();
    private readonly DataGridView _dgvOrders = CreateGrid();
    private readonly DataGridView _dgvOrderItems = CreateGrid();
    private readonly DataGridView _dgvPayments = CreateGrid();
    private readonly DataGridView _dgvSales = CreateGrid();
    private readonly DataGridView _dgvStock = CreateGrid();
    private readonly DataGridView _dgvCustomerSummary = CreateGrid();

    public MainForm()
    {
        Text = "IPB2 Online Book Store System";
        Width = 1400;
        Height = 840;
        StartPosition = FormStartPosition.CenterScreen;

        BuildManagementTab();
        BuildOrderTab();
        BuildPaymentTab();
        BuildReportTab();

        Controls.Add(_tabs);
        Load += (_, _) => RefreshAll();
    }

    private void BuildManagementTab()
    {
        var page = new TabPage("Management");
        var split = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2 };
        split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        split.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        split.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        split.Controls.Add(CreateEntityPanel("Authors", _dgvAuthors, CreateAuthor, UpdateAuthor, DeleteAuthor), 0, 0);
        split.Controls.Add(CreateEntityPanel("Categories", _dgvCategories, CreateCategory, UpdateCategory, DeleteCategory), 1, 0);
        split.Controls.Add(CreateEntityPanel("Customers", _dgvCustomers, CreateCustomer, UpdateCustomer, DeleteCustomer), 0, 1);
        split.Controls.Add(CreateEntityPanel("Books", _dgvBooks, CreateBook, UpdateBook, DeleteBook), 1, 1);

        page.Controls.Add(split);
        _tabs.TabPages.Add(page);
    }

    private void BuildOrderTab()
    {
        var page = new TabPage("Orders");
        var root = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal, SplitterDistance = 330 };

        var top = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true };
        var createOrderButton = new Button { Text = "Create Order", Width = 120 };
        createOrderButton.Click += (_, _) => CreateOrder();

        var addItemButton = new Button { Text = "Add Order Item", Width = 120 };
        addItemButton.Click += (_, _) => AddOrderItem();

        var updateStatusButton = new Button { Text = "Update Order", Width = 120 };
        updateStatusButton.Click += (_, _) => UpdateOrder();

        var cancelOrderButton = new Button { Text = "Cancel Order", Width = 120 };
        cancelOrderButton.Click += (_, _) => CancelOrder();

        var refreshOrdersButton = new Button { Text = "Refresh", Width = 100 };
        refreshOrdersButton.Click += (_, _) => RefreshOrders();

        top.Controls.AddRange([createOrderButton, addItemButton, updateStatusButton, cancelOrderButton, refreshOrdersButton]);

        var ordersPanel = new Panel { Dock = DockStyle.Fill };
        ordersPanel.Controls.Add(_dgvOrders);
        ordersPanel.Controls.Add(top);

        root.Panel1.Controls.Add(ordersPanel);
        root.Panel2.Controls.Add(_dgvOrderItems);

        _dgvOrders.SelectionChanged += (_, _) => LoadSelectedOrderItems();

        page.Controls.Add(root);
        _tabs.TabPages.Add(page);
    }

    private void BuildPaymentTab()
    {
        var page = new TabPage("Payments");
        var panel = new Panel { Dock = DockStyle.Fill };
        var controls = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true };

        var calculateButton = new Button { Text = "Calculate Total", Width = 130 };
        calculateButton.Click += (_, _) => CalculateTotal();

        var makePaymentButton = new Button { Text = "Make Payment", Width = 130 };
        makePaymentButton.Click += (_, _) => MakePayment();

        var receiptButton = new Button { Text = "Generate Receipt", Width = 130 };
        receiptButton.Click += (_, _) => GenerateReceipt();

        controls.Controls.AddRange([calculateButton, makePaymentButton, receiptButton]);

        panel.Controls.Add(_dgvPayments);
        panel.Controls.Add(controls);

        page.Controls.Add(panel);
        _tabs.TabPages.Add(page);
    }

    private void BuildReportTab()
    {
        var page = new TabPage("Reports");
        var split = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
        split.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        split.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        split.RowStyles.Add(new RowStyle(SizeType.Percent, 33.34F));

        split.Controls.Add(CreateReportPanel("Sales Report", _dgvSales, () => _dgvSales.DataSource = _service.GetSalesReport()), 0, 0);
        split.Controls.Add(CreateReportPanel("Book Stock Report", _dgvStock, () => _dgvStock.DataSource = _service.GetStockReport()), 0, 1);
        split.Controls.Add(CreateReportPanel("Customer Order Summary", _dgvCustomerSummary, () => _dgvCustomerSummary.DataSource = _service.GetCustomerOrderSummary()), 0, 2);

        page.Controls.Add(split);
        _tabs.TabPages.Add(page);
    }

    private static DataGridView CreateGrid() => new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false
    };

    private Panel CreateEntityPanel(string title, DataGridView grid, Action createAction, Action updateAction, Action deleteAction)
    {
        var wrapper = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8) };
        var header = new Label { Text = title, Dock = DockStyle.Top, Height = 26, Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold) };

        var toolbar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40 };
        var addButton = new Button { Text = "Add", Width = 80 };
        var editButton = new Button { Text = "Edit", Width = 80 };
        var deleteButton = new Button { Text = "Delete", Width = 80 };
        var refreshButton = new Button { Text = "Refresh", Width = 80 };

        addButton.Click += (_, _) => createAction();
        editButton.Click += (_, _) => updateAction();
        deleteButton.Click += (_, _) => deleteAction();
        refreshButton.Click += (_, _) => RefreshAll();

        toolbar.Controls.AddRange([addButton, editButton, deleteButton, refreshButton]);

        wrapper.Controls.Add(grid);
        wrapper.Controls.Add(toolbar);
        wrapper.Controls.Add(header);
        return wrapper;
    }

    private static Panel CreateReportPanel(string title, DataGridView grid, Action refreshAction)
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8) };
        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40 };
        top.Controls.Add(new Label { Text = title, Width = 200, Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold) });
        var btn = new Button { Text = "Refresh", Width = 90 };
        btn.Click += (_, _) => refreshAction();
        top.Controls.Add(btn);

        panel.Controls.Add(grid);
        panel.Controls.Add(top);
        return panel;
    }

    private void RefreshAll()
    {
        _dgvAuthors.DataSource = _service.GetAuthors();
        _dgvCategories.DataSource = _service.GetCategories();
        _dgvCustomers.DataSource = _service.GetCustomers();
        _dgvBooks.DataSource = _service.GetBooks();
        RefreshOrders();
        _dgvSales.DataSource = _service.GetSalesReport();
        _dgvStock.DataSource = _service.GetStockReport();
        _dgvCustomerSummary.DataSource = _service.GetCustomerOrderSummary();
    }

    private void RefreshOrders()
    {
        _dgvOrders.DataSource = _service.GetOrders();
        LoadSelectedOrderItems();
    }

    private int? SelectedInt(DataGridView dgv, string column)
    {
        if (dgv.CurrentRow?.Cells[column].Value is null) return null;
        return Convert.ToInt32(dgv.CurrentRow.Cells[column].Value);
    }

    private string? Prompt(string title, string label, string initial = "")
    {
        using var f = new Form { Width = 420, Height = 170, Text = title, StartPosition = FormStartPosition.CenterParent };
        var l = new Label { Left = 10, Top = 20, Text = label, Width = 380 };
        var tb = new TextBox { Left = 10, Top = 45, Width = 380, Text = initial };
        var ok = new Button { Text = "OK", Left = 230, Width = 75, Top = 80, DialogResult = DialogResult.OK };
        var cancel = new Button { Text = "Cancel", Left = 315, Width = 75, Top = 80, DialogResult = DialogResult.Cancel };
        f.Controls.AddRange([l, tb, ok, cancel]);
        f.AcceptButton = ok;
        f.CancelButton = cancel;
        return f.ShowDialog(this) == DialogResult.OK ? tb.Text.Trim() : null;
    }

    private void CreateAuthor()
    {
        var name = Prompt("New Author", "Author Name:");
        if (string.IsNullOrWhiteSpace(name)) return;
        var bio = Prompt("New Author", "Bio (optional):");
        _service.CreateAuthor(name, bio);
        _dgvAuthors.DataSource = _service.GetAuthors();
    }

    private void UpdateAuthor()
    {
        var id = SelectedInt(_dgvAuthors, "AuthorId");
        if (id is null) return;
        var name = Prompt("Edit Author", "Author Name:", _dgvAuthors.CurrentRow!.Cells["AuthorName"].Value?.ToString() ?? "");
        if (string.IsNullOrWhiteSpace(name)) return;
        var bio = Prompt("Edit Author", "Bio (optional):", _dgvAuthors.CurrentRow!.Cells["Bio"].Value?.ToString() ?? "");
        _service.UpdateAuthor(id.Value, name, bio);
        _dgvAuthors.DataSource = _service.GetAuthors();
    }

    private void DeleteAuthor()
    {
        var id = SelectedInt(_dgvAuthors, "AuthorId");
        if (id is null) return;
        _service.DeleteAuthor(id.Value);
        _dgvAuthors.DataSource = _service.GetAuthors();
    }

    private void CreateCategory()
    {
        var name = Prompt("New Category", "Category Name:");
        if (string.IsNullOrWhiteSpace(name)) return;
        var desc = Prompt("New Category", "Description (optional):");
        _service.CreateCategory(name, desc);
        _dgvCategories.DataSource = _service.GetCategories();
    }

    private void UpdateCategory()
    {
        var id = SelectedInt(_dgvCategories, "CategoryId");
        if (id is null) return;
        var name = Prompt("Edit Category", "Category Name:", _dgvCategories.CurrentRow!.Cells["CategoryName"].Value?.ToString() ?? "");
        if (string.IsNullOrWhiteSpace(name)) return;
        var desc = Prompt("Edit Category", "Description (optional):", _dgvCategories.CurrentRow!.Cells["Description"].Value?.ToString() ?? "");
        _service.UpdateCategory(id.Value, name, desc);
        _dgvCategories.DataSource = _service.GetCategories();
    }

    private void DeleteCategory()
    {
        var id = SelectedInt(_dgvCategories, "CategoryId");
        if (id is null) return;
        _service.DeleteCategory(id.Value);
        _dgvCategories.DataSource = _service.GetCategories();
    }

    private void CreateCustomer()
    {
        var name = Prompt("New Customer", "Full Name:");
        if (string.IsNullOrWhiteSpace(name)) return;
        var email = Prompt("New Customer", "Email:");
        var phone = Prompt("New Customer", "Phone:");
        var address = Prompt("New Customer", "Address:");
        _service.CreateCustomer(name, email, phone, address);
        _dgvCustomers.DataSource = _service.GetCustomers();
    }

    private void UpdateCustomer()
    {
        var id = SelectedInt(_dgvCustomers, "CustomerId");
        if (id is null) return;
        var row = _dgvCustomers.CurrentRow!;
        var name = Prompt("Edit Customer", "Full Name:", row.Cells["FullName"].Value?.ToString() ?? "");
        if (string.IsNullOrWhiteSpace(name)) return;
        var email = Prompt("Edit Customer", "Email:", row.Cells["Email"].Value?.ToString() ?? "");
        var phone = Prompt("Edit Customer", "Phone:", row.Cells["Phone"].Value?.ToString() ?? "");
        var address = Prompt("Edit Customer", "Address:", row.Cells["Address"].Value?.ToString() ?? "");
        _service.UpdateCustomer(id.Value, name, email, phone, address);
        _dgvCustomers.DataSource = _service.GetCustomers();
    }

    private void DeleteCustomer()
    {
        var id = SelectedInt(_dgvCustomers, "CustomerId");
        if (id is null) return;
        _service.DeleteCustomer(id.Value);
        _dgvCustomers.DataSource = _service.GetCustomers();
    }

    private void CreateBook()
    {
        var title = Prompt("New Book", "Title:");
        if (string.IsNullOrWhiteSpace(title)) return;

        var authorId = Prompt("New Book", "AuthorId:");
        var categoryId = Prompt("New Book", "CategoryId:");
        var price = Prompt("New Book", "Price:", "0");
        var stock = Prompt("New Book", "Stock:", "0");
        var year = Prompt("New Book", "Published Year:");
        var isbn = Prompt("New Book", "ISBN:");

        _service.CreateBook(
            title,
            int.Parse(authorId ?? "0"),
            int.Parse(categoryId ?? "0"),
            decimal.Parse(price ?? "0"),
            int.Parse(stock ?? "0"),
            int.TryParse(year, out var parsedYear) ? parsedYear : null,
            isbn);

        _dgvBooks.DataSource = _service.GetBooks();
    }

    private void UpdateBook()
    {
        var id = SelectedInt(_dgvBooks, "BookId");
        if (id is null) return;
        var row = _dgvBooks.CurrentRow!;

        var title = Prompt("Edit Book", "Title:", row.Cells["Title"].Value?.ToString() ?? "");
        if (string.IsNullOrWhiteSpace(title)) return;

        var authorId = Prompt("Edit Book", "AuthorId:", row.Cells["AuthorId"].Value?.ToString() ?? "");
        var categoryId = Prompt("Edit Book", "CategoryId:", row.Cells["CategoryId"].Value?.ToString() ?? "");
        var price = Prompt("Edit Book", "Price:", row.Cells["Price"].Value?.ToString() ?? "0");
        var stock = Prompt("Edit Book", "Stock:", row.Cells["Stock"].Value?.ToString() ?? "0");
        var year = Prompt("Edit Book", "Published Year:", row.Cells["PublishedYear"].Value?.ToString() ?? "");
        var isbn = Prompt("Edit Book", "ISBN:", row.Cells["ISBN"].Value?.ToString() ?? "");

        _service.UpdateBook(
            id.Value,
            title,
            int.Parse(authorId ?? "0"),
            int.Parse(categoryId ?? "0"),
            decimal.Parse(price ?? "0"),
            int.Parse(stock ?? "0"),
            int.TryParse(year, out var parsedYear) ? parsedYear : null,
            isbn);

        _dgvBooks.DataSource = _service.GetBooks();
    }

    private void DeleteBook()
    {
        var id = SelectedInt(_dgvBooks, "BookId");
        if (id is null) return;
        _service.DeleteBook(id.Value);
        _dgvBooks.DataSource = _service.GetBooks();
    }

    private void CreateOrder()
    {
        var customerId = Prompt("Create Order", "CustomerId:");
        if (!int.TryParse(customerId, out var id)) return;
        _service.CreateOrder(id);
        RefreshOrders();
    }

    private void AddOrderItem()
    {
        var orderId = SelectedInt(_dgvOrders, "OrderId");
        if (orderId is null) return;

        var bookId = Prompt("Add Item", "BookId:");
        var qty = Prompt("Add Item", "Quantity:", "1");
        if (!int.TryParse(bookId, out var bid) || !int.TryParse(qty, out var q) || q <= 0)
        {
            MessageBox.Show("Invalid BookId or Quantity.");
            return;
        }

        _service.AddOrderItem(orderId.Value, bid, q);
        RefreshOrders();
    }

    private void UpdateOrder()
    {
        var orderId = SelectedInt(_dgvOrders, "OrderId");
        if (orderId is null) return;
        var status = Prompt("Update Order", "Status (Pending/Paid/Cancelled):", _dgvOrders.CurrentRow!.Cells["Status"].Value?.ToString() ?? "Pending");
        if (string.IsNullOrWhiteSpace(status)) return;
        _service.UpdateOrderStatus(orderId.Value, status);
        RefreshOrders();
    }

    private void CancelOrder()
    {
        var orderId = SelectedInt(_dgvOrders, "OrderId");
        if (orderId is null) return;
        _service.CancelOrder(orderId.Value);
        RefreshOrders();
    }

    private void LoadSelectedOrderItems()
    {
        var orderId = SelectedInt(_dgvOrders, "OrderId");
        if (orderId is null)
        {
            _dgvOrderItems.DataSource = new DataTable();
            return;
        }

        _dgvOrderItems.DataSource = _service.GetOrderItems(orderId.Value);
        _dgvPayments.DataSource = _service.GetPaymentsByOrder(orderId.Value);
    }

    private void CalculateTotal()
    {
        var orderId = SelectedInt(_dgvOrders, "OrderId");
        if (orderId is null) return;

        var order = _service.GetOrders();
        var row = order.AsEnumerable().FirstOrDefault(x => Convert.ToInt32(x["OrderId"]) == orderId.Value);
        if (row is null) return;

        MessageBox.Show($"Order #{orderId} Total: {row["TotalAmount"]:C2}");
    }

    private void MakePayment()
    {
        var orderId = SelectedInt(_dgvOrders, "OrderId");
        if (orderId is null) return;
        var amount = Prompt("Make Payment", "Amount:", _dgvOrders.CurrentRow!.Cells["TotalAmount"].Value?.ToString() ?? "0");
        var method = Prompt("Make Payment", "Payment Method (Cash/Card/QR):", "Cash");
        if (!decimal.TryParse(amount, out var payAmount) || payAmount <= 0 || string.IsNullOrWhiteSpace(method))
        {
            MessageBox.Show("Invalid payment data.");
            return;
        }

        _service.MakePayment(orderId.Value, payAmount, method);
        _dgvPayments.DataSource = _service.GetPaymentsByOrder(orderId.Value);
        RefreshOrders();
    }

    private void GenerateReceipt()
    {
        var orderId = SelectedInt(_dgvOrders, "OrderId");
        if (orderId is null) return;
        var orderRow = _dgvOrders.CurrentRow!;
        var payments = _service.GetPaymentsByOrder(orderId.Value);

        var paid = payments.AsEnumerable().Sum(x => Convert.ToDecimal(x["Amount"]));
        var total = Convert.ToDecimal(orderRow.Cells["TotalAmount"].Value);
        var customer = orderRow.Cells["FullName"].Value?.ToString();

        MessageBox.Show($"Receipt\nOrder: {orderId}\nCustomer: {customer}\nTotal: {total:C2}\nPaid: {paid:C2}\nBalance: {(total - paid):C2}");
    }
}
