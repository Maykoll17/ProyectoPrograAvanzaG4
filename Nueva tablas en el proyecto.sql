
----ejecutar esta nueva tablas y refeecar tambien el digram en visual para que
--no le de error

CREATE TABLE AreaRecreativa (
    ID_Area INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(MAX),
    Horario NVARCHAR(100)
);
