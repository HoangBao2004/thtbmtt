namespace ShopNoiThat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFailedLoginAndOtpColumns : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.category",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 255),
                        slug = c.String(maxLength: 255),
                        parentid = c.Int(nullable: false),
                        orders = c.Int(nullable: false),
                        metakey = c.String(maxLength: 150),
                        metadesc = c.String(maxLength: 150),
                        created_at = c.DateTime(storeType: "smalldatetime"),
                        created_by = c.Int(),
                        updated_at = c.DateTime(storeType: "smalldatetime"),
                        updated_by = c.Int(),
                        status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.contact",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        fullname = c.String(maxLength: 100),
                        email = c.String(maxLength: 100),
                        phone = c.String(maxLength: 15),
                        title = c.String(maxLength: 255),
                        detail = c.String(storeType: "ntext"),
                        created_at = c.DateTime(storeType: "smalldatetime"),
                        created_by = c.Int(),
                        updated_at = c.DateTime(storeType: "smalldatetime"),
                        updated_by = c.Int(),
                        status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.link",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        slug = c.String(),
                        tableId = c.Int(nullable: false),
                        type = c.String(),
                        parentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.menu",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 255),
                        type = c.String(maxLength: 255),
                        link = c.String(maxLength: 255),
                        tableid = c.Int(),
                        parentid = c.Int(nullable: false),
                        orders = c.Int(nullable: false),
                        position = c.String(maxLength: 255),
                        created_at = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        created_by = c.Int(),
                        updated_at = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        updated_by = c.Int(),
                        status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ordersdetail",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        orderid = c.Int(nullable: false),
                        productid = c.Int(nullable: false),
                        price = c.Double(nullable: false),
                        quantity = c.Int(nullable: false),
                        priceSale = c.Int(nullable: false),
                        amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.order",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        code = c.String(),
                        userid = c.Int(nullable: false),
                        created_ate = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        exportdate = c.DateTime(storeType: "smalldatetime"),
                        deliveryaddress = c.String(maxLength: 255),
                        deliveryname = c.String(maxLength: 100),
                        deliveryphone = c.String(maxLength: 255),
                        deliveryemail = c.String(maxLength: 255),
                        deliveryPaymentMethod = c.String(),
                        StatusPayment = c.Int(nullable: false),
                        updated_at = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        updated_by = c.Int(),
                        status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.post",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        topid = c.Int(),
                        title = c.String(),
                        slug = c.String(maxLength: 255),
                        detail = c.String(storeType: "ntext"),
                        img = c.String(maxLength: 255),
                        type = c.String(maxLength: 50),
                        metakey = c.String(maxLength: 150),
                        metadesc = c.String(maxLength: 150),
                        created_at = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        created_by = c.Int(nullable: false),
                        updated_at = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        updated_by = c.Int(nullable: false),
                        status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.product",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        catid = c.Int(nullable: false),
                        Submenu = c.Int(nullable: false),
                        name = c.String(),
                        slug = c.String(maxLength: 255),
                        img = c.String(maxLength: 100),
                        detail = c.String(storeType: "ntext"),
                        number = c.Int(nullable: false),
                        price = c.Double(nullable: false),
                        pricesale = c.Double(nullable: false),
                        metakey = c.String(maxLength: 150),
                        metadesc = c.String(),
                        created_at = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        created_by = c.Int(nullable: false),
                        updated_at = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        updated_by = c.Int(nullable: false),
                        status = c.Int(nullable: false),
                        sold = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.role",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        parentId = c.Int(nullable: false),
                        accessName = c.String(maxLength: 255),
                        description = c.String(maxLength: 225),
                        GropID = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.slider",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 255),
                        url = c.String(maxLength: 255),
                        position = c.String(maxLength: 100),
                        img = c.String(maxLength: 100),
                        orders = c.Int(),
                        created_at = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        created_by = c.Int(),
                        updated_at = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        updated_by = c.Int(),
                        status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.topic",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 255),
                        slug = c.String(maxLength: 255),
                        parentid = c.Int(nullable: false),
                        orders = c.Int(nullable: false),
                        metakey = c.String(maxLength: 150),
                        metadesc = c.String(),
                        created_at = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        created_by = c.Int(nullable: false),
                        updated_at = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        updated_by = c.Int(nullable: false),
                        status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.user",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        fullname = c.String(maxLength: 255),
                        username = c.String(maxLength: 225),
                        password = c.String(maxLength: 64),
                        email = c.String(maxLength: 255),
                        gender = c.String(maxLength: 5),
                        phone = c.String(maxLength: 20),
                        img = c.String(maxLength: 100),
                        access = c.Int(nullable: false),
                        created_at = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        created_by = c.Int(nullable: false),
                        updated_at = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        updated_by = c.Int(nullable: false),
                        status = c.Int(nullable: false),
                        locked_until = c.DateTime(storeType: "smalldatetime"),
                        failed_login = c.Int(),
                        last_failed_login = c.DateTime(storeType: "smalldatetime"),
                        otp_code = c.String(maxLength: 6),
                        otp_created_at = c.DateTime(storeType: "smalldatetime"),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.user");
            DropTable("dbo.topic");
            DropTable("dbo.slider");
            DropTable("dbo.role");
            DropTable("dbo.product");
            DropTable("dbo.post");
            DropTable("dbo.order");
            DropTable("dbo.ordersdetail");
            DropTable("dbo.menu");
            DropTable("dbo.link");
            DropTable("dbo.contact");
            DropTable("dbo.category");
        }
    }
}
