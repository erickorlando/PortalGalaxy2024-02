using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortalGalaxy.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SpListaInscripciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE uspListarInscripciones
(
	@InstructorId INT = NULL,
	@Taller NVARCHAR(200) = NULL,
	@Situacion INT = NULL,
	@FechaInicio DATE = NULL,
	@FechaFin DATE = NULL,
	@Pagina INT = 0,
	@Filas INT = 5
)
AS
BEGIN

	SELECT 
		t.Id,
		T.Nombre Taller,
		C.Nombre Categoria,
		INS.Nombres Instructor,
		CONVERT(VARCHAR, T.FechaInicio, 103) Fecha,
		CASE t.Situacion
			WHEN 0 THEN 'Por Aperturar'
			WHEN 1 THEN 'Aperturada'
			WHEN 2 THEN 'Concluida'
			WHEN 3 THEN 'Cancelada'
		END Situacion,
		COUNT(*) Cantidad
	FROM Inscripcion I
	INNER JOIN Taller t ON I.TallerId = t.Id
	INNER JOIN Categoria c ON T.CategoriaId = c.Id
	INNER JOIN Instructor INS ON T.InstructorId = INS.Id
	WHERE I.Estado = 1
	AND (@InstructorId IS NULL OR T.InstructorId = @InstructorId)
	AND (@Taller IS NULL OR T.Nombre LIKE '%' + @Taller + '%')
	AND (@Situacion IS NULL OR T.Situacion = @Situacion)
	AND (@FechaInicio IS NULL OR (T.FechaInicio BETWEEN @FechaInicio AND @FechaFin))
	GROUP BY T.ID, T.Nombre, C.Nombre, INS.Nombres, T.FechaInicio, T.Situacion
	ORDER BY 1
	OFFSET @Pagina ROWS FETCH NEXT @Filas ROWS ONLY;

	SELECT 
		COUNT(*) Total
	FROM Inscripcion I
	INNER JOIN Taller t ON I.TallerId = t.Id
	INNER JOIN Categoria c ON T.CategoriaId = c.Id
	INNER JOIN Instructor INS ON T.InstructorId = INS.Id
	WHERE I.Estado = 1
	AND (@InstructorId IS NULL OR T.InstructorId = @InstructorId)
	AND (@Taller IS NULL OR T.Nombre LIKE '%' + @Taller + '%')
	AND (@Situacion IS NULL OR T.Situacion = @Situacion)
	AND (@FechaInicio IS NULL OR (T.FechaInicio BETWEEN @FechaInicio AND @FechaFin))

END
GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROC uspListarInscripciones");
        }
    }
}
