using TuberTreats.Models;
using TuberTreats.Models.DTOs;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

List<TuberDriver> tuberDrivers = new List<TuberDriver>()
{
    new TuberDriver()
    {
        Id = 1,
        Name = "Scott Hanson"
    },
    new TuberDriver()
    {
        Id = 2,
        Name = "Matthew Mercer"
    },
    new TuberDriver()
    {
        Id = 3,
        Name = "Bethany Smith"
    }
};

List<Customer> customers = new List<Customer>()
{
    new Customer()
    {
        Id = 1,
        Name = "Sabrina Carpenter",
        Address = "123 Espresso Ave."
    },
    new Customer()
    {
        Id = 2,
        Name = "Emily Axeford",
        Address = "55 New York St."
    },
    new Customer()
    {
        Id = 3,
        Name = "Troy Baker",
        Address = "1258 USA Dr."
    },
    new Customer()
    {
        Id = 3,
        Name = "John Shephard",
        Address = "1 Normandy Ln."
    },
    new Customer()
    {
        Id = 4,
        Name = "Jen Meyers",
        Address = "5555 South St."
    },
    new Customer()
    {
        Id = 5,
        Name = "Samuel Winchester",
        Address = "1234 Kansas Ks."
    }
};

List<Topping> toppings = new List<Topping>()
{
    new Topping()
    {
        Id = 1,
        Name = "Bacon"
    },
    new Topping()
    {
        Id = 2,
        Name = "Cheese"
    },
    new Topping()
    {
        Id = 3,
        Name = "Scallions"
    },
    new Topping()
    {
        Id = 4,
        Name = "Pepperoni"
    },
    new Topping()
    {
        Id = 5,
        Name = "Mushrooms"
    }
};

List<TuberOrder> orders = new List<TuberOrder>
{
    new TuberOrder()
    {
        Id = 1,
        OrderPlacedOnDate = new DateTime(2024, 1, 24),
        CustomerId = 2,
        TuberDriverId = 1,
        DeliveredOnDate = new DateTime(2024, 1, 25)
    },
    new TuberOrder()
    {
        Id = 2,
        OrderPlacedOnDate = new DateTime(2024, 8, 15),
        CustomerId = 1,
        TuberDriverId = 2,
        DeliveredOnDate = new DateTime(2024, 8, 15)
       
    },
    new TuberOrder()
    {
        Id = 3,
        OrderPlacedOnDate = new DateTime(2024, 12, 18),
        CustomerId = 4
      }
};

List<TuberTopping> tuberToppings = new List<TuberTopping>()
{
    new TuberTopping()
    {
        Id = 1,
        TuberOrderId = 1,
        ToppingId = 2
    },
    new TuberTopping()
    {
        Id = 2,
        TuberOrderId = 1,
        ToppingId = 4
    },
    new TuberTopping()
    {
        Id = 3,
        TuberOrderId = 2,
        ToppingId = 1
    },
    new TuberTopping()
    {
        Id = 4,
        TuberOrderId = 2,
        ToppingId = 2
    },
    new TuberTopping()
    {
        Id = 5,
        TuberOrderId = 2,
        ToppingId = 5
    },
    new TuberTopping()
    {
        Id = 6,
        TuberOrderId = 3,
        ToppingId = 2
    }
};

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//add endpoints here

//-------->GET Endpoints<----------

app.MapGet("/tuberorders" , () =>
{

     return orders.Select(o => 
    {

        List<TuberTopping> orderToppings = tuberToppings.Where(tt => tt.TuberOrderId == o.Id).ToList();
        return new TubeOrderDTO 
        {
        Id = o.Id,
        OrderPlacedOnDate = o.OrderPlacedOnDate,
        CustomerId = o.CustomerId,
        TuberDriverId = o.TuberDriverId,
        DeliveredOnDate = o.DeliveredOnDate,
        Toppings = orderToppings.Select(ot => new ToppingDTO
        {
            Id = toppings.FirstOrDefault(t => ot.ToppingId == t.Id).Id,
            Name = toppings.FirstOrDefault(t => ot.ToppingId == t.Id).Name
        }).ToList()
        };
    }).ToList();
});

app.MapGet("/tuberorders/{id}", (int id) =>
{
    TuberOrder chosenOrder = orders.FirstOrDefault(o => o.Id == id);
    List<TuberTopping> orderToppings = tuberToppings.Where(tt => tt.TuberOrderId == chosenOrder.Id).ToList();

    if(chosenOrder == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new TubeOrderDTO
    {
        Id = chosenOrder.Id,
        OrderPlacedOnDate = chosenOrder.OrderPlacedOnDate,
        CustomerId = chosenOrder.CustomerId,
        TuberDriverId = chosenOrder.TuberDriverId,
        DeliveredOnDate = chosenOrder.DeliveredOnDate,
        Toppings = orderToppings.Select(ot => new ToppingDTO
        {
            Id = toppings.FirstOrDefault(t => t.Id == ot.ToppingId).Id,
            Name = toppings.FirstOrDefault(t => t.Id == ot.ToppingId).Name
        }).ToList()
        
    });

});

app.MapGet("/toppings", () =>
{
    return toppings.Select(t => new ToppingDTO
    {
        Id = t.Id,
        Name = t.Name
    });
});

app.MapGet("/toppings/{id}", (int id) => 
{
    Topping selectedTopping = toppings.FirstOrDefault(t => t.Id == id);

    if (selectedTopping == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new ToppingDTO 
    {
        Id = selectedTopping.Id,
        Name = selectedTopping.Name
    });
});


app.MapGet("/tubertoppings", () =>
{
    return tuberToppings.Select(tt => new TuberTopping
    {
        Id = tt.Id,
        TuberOrderId = tt.TuberOrderId,
        ToppingId = tt.ToppingId
    });
});

app.MapGet("/customers", () =>
{
    return customers.Select(c => new CustomerDTO
    {
        Id = c.Id,
        Name = c.Name,
        Address = c.Address
    });
});

app.MapGet("/customers/{id}", (int id) =>
{
    Customer foundCustomer = customers.FirstOrDefault(c => c.Id == id);
    List<TuberOrder> customerOrders = orders.Where(co => co.CustomerId == id).ToList();

    if (foundCustomer == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new CustomerDTO
    {
        Id = foundCustomer.Id,
        Name = foundCustomer.Name,
        Address = foundCustomer.Address,
        TuberOrders = customerOrders.Select(co => new TubeOrderDTO
        {
            Id = co.Id,
            OrderPlacedOnDate = co.OrderPlacedOnDate,
            CustomerId = co.CustomerId,
            TuberDriverId = co.TuberDriverId,
            DeliveredOnDate = co.DeliveredOnDate
        }).ToList()
    });
});

app.MapGet("/tuberdrivers", () =>
{
    return tuberDrivers.Select(td => new TuberDriverDTO
    {
        ID = td.Id,
        Name = td.Name
    });
});

app.MapGet("/tuberdrivers/{id}", (int id) =>
{
    TuberDriver foundDriver = tuberDrivers.FirstOrDefault(td => td.Id == id);
    List<TuberOrder> driverDeliveries = orders.Where(dd => dd.TuberDriverId == id).ToList();

    if (foundDriver == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new TuberDriverDTO
    {
        ID = foundDriver.Id,
        Name = foundDriver.Name,
        TuberDeliveries = driverDeliveries.Select(dd => new TubeOrderDTO
        {
            Id = dd.Id,
            OrderPlacedOnDate = dd.OrderPlacedOnDate,
            CustomerId = dd.CustomerId,
            TuberDriverId = id,
            DeliveredOnDate = dd.DeliveredOnDate
        }).ToList()
    });

});

//--------->POST Endpoints<----------

app.MapPost("/tuberorders", (TuberOrder order) =>
{
    order.Id = orders.Max(o => o.Id) + 1;
    order.OrderPlacedOnDate = DateTime.Now;
    orders.Add(order);

    return Results.Created($"/tuberorders/{order.Id}", new TubeOrderDTO
    {
        Id = order.Id,
        OrderPlacedOnDate = order.OrderPlacedOnDate,
        CustomerId = order.CustomerId,
    });
    
});

app.MapPost("/tuberorders/{id}/complete", (int id) =>
{
    TuberOrder orderToComplete = orders.FirstOrDefault(o => o.Id == id);

    if(orderToComplete == null)
    {
        return Results.NotFound();
    }

    orderToComplete.DeliveredOnDate = DateTime.Now;

    return Results.Ok(orderToComplete);
});

app.MapPost("/tubertoppings", (TuberTopping orderTopping) =>
{

    orderTopping.Id = tuberToppings.Max(tt => tt.Id) + 1;

   TuberTopping newTopping = new TuberTopping
   {
        Id = orderTopping.Id,
        TuberOrderId = orderTopping.TuberOrderId,
        ToppingId = orderTopping.ToppingId
   };

   tuberToppings.Add(newTopping);

   return Results.Created($"tubertoppings/{newTopping.Id}", newTopping);
});

app.MapPost("/customers", (Customer customer) =>
{
    customer.Id = customers.Max(c => c.Id) + 1;

    customers.Add(customer);

    return Results.Created($"/customers/{customer.Id}", new CustomerDTO
    {
        Id = customer.Id,
        Name = customer.Name,
        Address = customer.Address
    });
});

//------->PUT Endpoints<------------

app.MapPut("/tuberorders/{id}", (int Id, TuberOrder order) => 
{
    TuberOrder selectedOrder = orders.FirstOrDefault(o => o.Id == Id);

    if(selectedOrder == null)
    {
        return Results.NotFound();
    }
    selectedOrder.Id = selectedOrder.Id;
    selectedOrder.OrderPlacedOnDate = order.OrderPlacedOnDate;
    selectedOrder.CustomerId = order.CustomerId;
    selectedOrder.TuberDriverId = order.TuberDriverId;

    return Results.Ok(order);

});

//------>DELETE Endpoints<--------

app.MapDelete("/tubertoppings/{id}", (int id) =>
{
    TuberTopping toppingToDelete = tuberToppings.FirstOrDefault(tt => tt.Id == id);

    tuberToppings.Remove(toppingToDelete);

    return Results.NoContent();
});

app.MapDelete("/customers/{id}", (int id) => 
{
    Customer customerToDelete = customers.FirstOrDefault(c => c.Id == id);

    if(customerToDelete == null)
    {
        return Results.NotFound();
    }

    customers.Remove(customerToDelete);
    return Results.NoContent();
});

app.Run();
//don't touch or move this!
public partial class Program { }