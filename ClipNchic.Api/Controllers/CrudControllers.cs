using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClipNchic.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _db;
    public RolesController(AppDbContext db) { _db = db; }

    [HttpGet]
    public Task<List<Role>> GetAll() => _db.Roles.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Role>> Get(int id)
    {
        var entity = await _db.Roles.FindAsync(id);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<Role>> Create(Role role)
    {
        _db.Roles.Add(role);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = role.RoleId }, role);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Role role)
    {
        if (id != role.RoleId) return BadRequest();
        _db.Entry(role).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Roles.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Roles.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db;
    public UsersController(AppDbContext db) { _db = db; }

    [HttpGet]
    public Task<List<User>> GetAll() => _db.Users.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(int id)
    {
        var entity = await _db.Users.FindAsync(id);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = user.UserId }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, User user)
    {
        if (id != user.UserId) return BadRequest();
        _db.Entry(user).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Users.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Users.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

//[ApiController]
//[Route("api/[controller]")]
//public class ProductsController : ControllerBase
//{
//    private readonly AppDbContext _db;
//    public ProductsController(AppDbContext<DirectedGraph xmlns="http://schemas.microsoft.com/vs/2009/dgml">
//  <Nodes>
//    <Node Id="(@1 @2)" Visibility="Hidden" />
//    <Node Id="(@3 Type=CheckoutDto)" Category="CodeSchema_Class" CodeSchemaProperty_IsPublic="True" CommonLabel="CheckoutDto" Icon="Microsoft.VisualStudio.Class.Public" IsDragSource="True" Label="CheckoutDto" SourceLocation="(Assembly=file:///C:/Users/cungt/Source/Repos/BE_clipNchick/ClipNchic.Api/Controllers/CartController.cs StartLineNumber=60 StartCharacterOffset=13 EndLineNumber=60 EndCharacterOffset=24)" />
//  </Nodes>
//  <Links>
//    <Link Source="(@1 @2)" Target="(@3 Type=CheckoutDto)" Category="Contains" />
//  </Links>
//  <Categories>
//    <Category Id="CodeSchema_Class" Label="Class" BasedOn="CodeSchema_Type" Icon="CodeSchema_Class" />
//    <Category Id="CodeSchema_Type" Label="Type" Icon="CodeSchema_Class" />
//    <Category Id="Contains" Label="Contains" Description="Whether the source of the link contains the target object" IsContainment="True" />
//  </Categories>
//  <Properties>
//    <Property Id="CodeSchemaProperty_IsPublic" Label="Is Public" Description="Flag to indicate the scope is Public" DataType="System.Boolean" />
//    <Property Id="CommonLabel" DataType="System.String" />
//    <Property Id="Icon" Label="Icon" DataType="System.String" />
//    <Property Id="IsContainment" DataType="System.Boolean" />
//    <Property Id="IsDragSource" Label="IsDragSource" Description="IsDragSource" DataType="System.Boolean" />
//    <Property Id="Label" Label="Label" Description="Displayable label of an Annotatable object" DataType="System.String" />
//    <Property Id="SourceLocation" Label="Start Line Number" DataType="Microsoft.VisualStudio.GraphModel.CodeSchema.SourceLocation" />
//    <Property Id="Visibility" Label="Visibility" Description="Defines whether a node in the graph is visible or not" DataType="System.Windows.Visibility" />
//  </Properties>
//  <QualifiedNames>
//    <Name Id="Assembly" Label="Assembly" ValueType="Uri" />
//    <Name Id="File" Label="File" ValueType="Uri" />
//    <Name Id="Type" Label="Type" ValueType="System.Object" />
//  </QualifiedNames>
//  <IdentifierAliases>
//    <Alias n="1" Uri="Assembly=$(VsSolutionUri)/ClipNchic.Api/ClipNchic.Api.csproj" />
//    <Alias n="2" Uri="File=$(VsSolutionUri)/ClipNchic.Api/Controllers/CartController.cs" />
//    <Alias n="3" Uri="Assembly=$(cee037fe-7fe8-4c20-8a77-48929d713ccc.OutputPathUri)" />
//  </IdentifierAliases>
//  <Paths>
//    <Path Id="cee037fe-7fe8-4c20-8a77-48929d713ccc.OutputPathUri" Value="file:///C:/Users/cungt/Source/Repos/BE_clipNchick/ClipNchic.Api/bin/Debug/net9.0/ClipNchic.Api.dll" />
//    <Path Id="VsSolutionUri" Value="file:///C:/Users/cungt/Source/Repos/BE_clipNchick" />
//  </Paths>
//</DirectedGraph> db) { _db = db; }

//    [HttpGet]
//    public Task<List<Product>> GetAll() => _db.Products.ToListAsync();

//    [HttpGet("{id}")]
//    public async Task<ActionResult<Product>> Get(int id)
//    {
//        var entity = await _db.Products.FindAsync(id);
//        return entity is null ? NotFound() : entity;
//    }

//    [HttpPost]
//    public async Task<ActionResult<Product>> Create(Product product)
//    {
//        _db.Products.Add(product);
//        await _db.SaveChangesAsync();
//        return CreatedAtAction(nameof(Get), new { id = product.ProductId }, product);
//    }

//    [HttpPut("{id}")]
//    public async Task<IActionResult> Update(int id, Product product)
//    {
//        if (id != product.ProductId) return BadRequest();
//        _db.Entry(product).State = EntityState.Modified;
//        await _db.SaveChangesAsync();
//        return NoContent();
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> Delete(int id)
//    {
//        var entity = await _db.Products.FindAsync(id);
//        if (entity is null) return NotFound();
//        _db.Products.Remove(entity);
//        await _db.SaveChangesAsync();
//        return NoContent();
//    }
//}

[ApiController]
[Route("api/[controller]")]
public class MaterialsController : ControllerBase
{
    private readonly AppDbContext _db;
    public MaterialsController(AppDbContext db) { _db = db; }

    [HttpGet]
    public Task<List<Material>> GetAll() => _db.Materials.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Material>> Get(int id)
    {
        var entity = await _db.Materials.FindAsync(id);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<Material>> Create(Material material)
    {
        _db.Materials.Add(material);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = material.MaterialId }, material);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Material material)
    {
        if (id != material.MaterialId) return BadRequest();
        _db.Entry(material).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Materials.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Materials.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
public class TexturesController : ControllerBase
{
    private readonly AppDbContext _db;
    public TexturesController(AppDbContext db) { _db = db; }

    [HttpGet]
    public Task<List<Texture>> GetAll() => _db.Textures.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Texture>> Get(int id)
    {
        var entity = await _db.Textures.FindAsync(id);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<Texture>> Create(Texture texture)
    {
        _db.Textures.Add(texture);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = texture.TextureId }, texture);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Texture texture)
    {
        if (id != texture.TextureId) return BadRequest();
        _db.Entry(texture).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Textures.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Textures.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
public class AccessoriesController : ControllerBase
{
    private readonly AppDbContext _db;
    public AccessoriesController(AppDbContext db) { _db = db; }

    [HttpGet]
    public Task<List<Accessory>> GetAll() => _db.Accessories.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Accessory>> Get(int id)
    {
        var entity = await _db.Accessories.FindAsync(id);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<Accessory>> Create(Accessory accessory)
    {
        _db.Accessories.Add(accessory);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = accessory.AccessoryId }, accessory);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Accessory accessory)
    {
        if (id != accessory.AccessoryId) return BadRequest();
        _db.Entry(accessory).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Accessories.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Accessories.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
public class ProductModelsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProductModelsController(AppDbContext db) { _db = db; }

    [HttpGet]
    public Task<List<ProductModel>> GetAll() => _db.ProductModels.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductModel>> Get(int id)
    {
        var entity = await _db.ProductModels.FindAsync(id);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<ProductModel>> Create(ProductModel model)
    {
        _db.ProductModels.Add(model);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = model.Model3DId }, model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductModel model)
    {
        if (id != model.Model3DId) return BadRequest();
        _db.Entry(model).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.ProductModels.FindAsync(id);
        if (entity is null) return NotFound();
        _db.ProductModels.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
public class CustomDesignsController : ControllerBase
{
    private readonly AppDbContext _db;
    public CustomDesignsController(AppDbContext db) { _db = db; }

    [HttpGet]
    public Task<List<CustomDesign>> GetAll() => _db.CustomDesigns.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomDesign>> Get(int id)
    {
        var entity = await _db.CustomDesigns.FindAsync(id);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<CustomDesign>> Create(CustomDesign design)
    {
        _db.CustomDesigns.Add(design);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = design.DesignId }, design);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CustomDesign design)
    {
        if (id != design.DesignId) return BadRequest();
        _db.Entry(design).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.CustomDesigns.FindAsync(id);
        if (entity is null) return NotFound();
        _db.CustomDesigns.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
public class CustomDesignAccessoriesController : ControllerBase
{
    private readonly AppDbContext _db;
    public CustomDesignAccessoriesController(AppDbContext db) { _db = db; }

    [HttpGet]
    public Task<List<CustomDesignAccessory>> GetAll() => _db.CustomDesignAccessories.ToListAsync();

    [HttpGet("{designId}/{accessoryId}")]
    public async Task<ActionResult<CustomDesignAccessory>> Get(int designId, int accessoryId)
    {
        var entity = await _db.CustomDesignAccessories.FindAsync(designId, accessoryId);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<CustomDesignAccessory>> Create(CustomDesignAccessory record)
    {
        _db.CustomDesignAccessories.Add(record);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { designId = record.DesignId, accessoryId = record.AccessoryId }, record);
    }

    [HttpPut("{designId}/{accessoryId}")]
    public async Task<IActionResult> Update(int designId, int accessoryId, CustomDesignAccessory record)
    {
        if (designId != record.DesignId || accessoryId != record.AccessoryId) return BadRequest();
        _db.Entry(record).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{designId}/{accessoryId}")]
    public async Task<IActionResult> Delete(int designId, int accessoryId)
    {
        var entity = await _db.CustomDesignAccessories.FindAsync(designId, accessoryId);
        if (entity is null) return NotFound();
        _db.CustomDesignAccessories.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;
    public OrdersController(AppDbContext db) { _db = db; }

    [HttpGet]
    public Task<List<Order>> GetAll() => _db.Orders.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> Get(int id)
    {
        var entity = await _db.Orders.FindAsync(id);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> Create(Order order)
    {
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = order.OrderId }, order);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Order order)
    {
        if (id != order.OrderId) return BadRequest();
        _db.Entry(order).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Orders.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Orders.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
public class OrderDetailsController : ControllerBase
{
    private readonly AppDbContext _db;
    public OrderDetailsController(AppDbContext db) { _db = db; }

    [HttpGet]
    public Task<List<OrderDetail>> GetAll() => _db.OrderDetails.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDetail>> Get(int id)
    {
        var entity = await _db.OrderDetails.FindAsync(id);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<OrderDetail>> Create(OrderDetail detail)
    {
        _db.OrderDetails.Add(detail);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = detail.OrderDetailId }, detail);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, OrderDetail detail)
    {
        if (id != detail.OrderDetailId) return BadRequest();
        _db.Entry(detail).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.OrderDetails.FindAsync(id);
        if (entity is null) return NotFound();
        _db.OrderDetails.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}


