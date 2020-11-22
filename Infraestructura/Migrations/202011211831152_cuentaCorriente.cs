namespace Infraestructura.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cuentaCorriente : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comprobante_CuentaCorriente",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        MovimientoCuentaCorriente_Id = c.Long(),
                        ClienteId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comprobante", t => t.Id)
                .ForeignKey("dbo.Movimiento_CuentaCorriente", t => t.MovimientoCuentaCorriente_Id)
                .ForeignKey("dbo.Persona_Cliente", t => t.ClienteId)
                .Index(t => t.Id)
                .Index(t => t.MovimientoCuentaCorriente_Id)
                .Index(t => t.ClienteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comprobante_CuentaCorriente", "ClienteId", "dbo.Persona_Cliente");
            DropForeignKey("dbo.Comprobante_CuentaCorriente", "MovimientoCuentaCorriente_Id", "dbo.Movimiento_CuentaCorriente");
            DropForeignKey("dbo.Comprobante_CuentaCorriente", "Id", "dbo.Comprobante");
            DropIndex("dbo.Comprobante_CuentaCorriente", new[] { "ClienteId" });
            DropIndex("dbo.Comprobante_CuentaCorriente", new[] { "MovimientoCuentaCorriente_Id" });
            DropIndex("dbo.Comprobante_CuentaCorriente", new[] { "Id" });
            DropTable("dbo.Comprobante_CuentaCorriente");
        }
    }
}
