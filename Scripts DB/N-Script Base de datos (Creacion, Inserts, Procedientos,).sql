-- Crear base de datos
IF DB_ID('SistemaAlquiler') IS NULL
    CREATE DATABASE SistemaAlquiler;
GO

USE SistemaAlquiler;
GO

-- Tabla: Edificio
CREATE TABLE Edificio (
    ID_Edificio INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Direccion VARCHAR(150) NOT NULL,
    Cantidad_Pisos INT NOT NULL
);

-- Tabla: Apartamento
CREATE TABLE Apartamento (
    ID_Apartamento INT IDENTITY(1,1) PRIMARY KEY,
    Codigo_Apartamento VARCHAR(8) NOT NULL UNIQUE,
    ID_Edificio INT NOT NULL,
    Piso INT NOT NULL,
    Metros_Cuadrados FLOAT NOT NULL,
    Cantidad_Habitantes INT,
    Cant_Sanitarios INT,
    Disponible BIT DEFAULT 1,
    FOREIGN KEY (ID_Edificio) REFERENCES Edificio(ID_Edificio)
);

-- Tabla: Cliente
CREATE TABLE Cliente (
    ID_Cliente INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(30) NOT NULL,
    Apellido VARCHAR(60) NOT NULL,
    Cedula VARCHAR(20) NOT NULL UNIQUE,
    Telefono VARCHAR(12),
    Correo VARCHAR(50),
    Fecha_Nacimiento DATE
);

-- Tabla: Aviso
CREATE TABLE Aviso (
    ID_Aviso INT IDENTITY(1,1) PRIMARY KEY,
    Titulo VARCHAR(100) NOT NULL,
    Contenido TEXT NOT NULL,
    Fecha_Publicacion DATE NOT NULL,
    Autor VARCHAR(100) NOT NULL
);

-- Relación: Aviso recibido por Cliente (Aviso_Cliente)
CREATE TABLE Aviso_Cliente (
    ID_Cliente INT NOT NULL,
    ID_Aviso INT NOT NULL,
    PRIMARY KEY (ID_Cliente, ID_Aviso),
    FOREIGN KEY (ID_Cliente) REFERENCES Cliente(ID_Cliente),
    FOREIGN KEY (ID_Aviso) REFERENCES Aviso(ID_Aviso)
);

-- Tabla: Contrato
CREATE TABLE Contrato (
    ID_Contrato INT IDENTITY(1,1) PRIMARY KEY,
    ID_Cliente INT NOT NULL,
    ID_Apartamento INT NOT NULL,
    Fecha_Inicio DATE NOT NULL,
    Fecha_Fin DATE NOT NULL,
    Monto_Mensual FLOAT NOT NULL,
    Estado VARCHAR(20) DEFAULT 'Activo',
    FOREIGN KEY (ID_Cliente) REFERENCES Cliente(ID_Cliente),
    FOREIGN KEY (ID_Apartamento) REFERENCES Apartamento(ID_Apartamento)
);

-- Tabla: Pago
CREATE TABLE Pago (
    ID_Pago INT IDENTITY(1,1) PRIMARY KEY,
    ID_Contrato INT NOT NULL,
    Fecha_Pago DATE NOT NULL,
    Monto_Pago FLOAT NOT NULL,
    Metodo_Pago VARCHAR(20),
    FOREIGN KEY (ID_Contrato) REFERENCES Contrato(ID_Contrato)
);

-- Tabla: Vehiculo
CREATE TABLE Vehiculo (
    ID_Vehiculo INT IDENTITY(1,1) PRIMARY KEY,
    ID_Cliente INT NOT NULL,
    Placa VARCHAR(20) NOT NULL UNIQUE,
    Marca VARCHAR(20),
    Modelo VARCHAR(20),
    Color VARCHAR(20),
    FOREIGN KEY (ID_Cliente) REFERENCES Cliente(ID_Cliente)
);

-- Tabla: Mantenimiento
CREATE TABLE Mantenimiento (
    ID_Mantenimiento INT IDENTITY(1,1) PRIMARY KEY,
    ID_Apartamento INT NOT NULL,
    Descripcion TEXT NOT NULL,
    Fecha_Mantenimiento DATE NOT NULL,
    Costo FLOAT NOT NULL,
    Tipo VARCHAR(20),
    FOREIGN KEY (ID_Apartamento) REFERENCES Apartamento(ID_Apartamento)
);

--Tabla: Area Recreativa

CREATE TABLE AreaRecreativa (
    ID_Area INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(MAX),
    Horario NVARCHAR(100)
);

CREATE TABLE Roll (
    ID_Roll INT PRIMARY KEY IDENTITY(1,1),
    Descripcion NVARCHAR(MAX),
);




----iNSERTS----------------------------------------------------------------------------------------




-- 1. Insertar Edificios
INSERT INTO Edificio (Nombre, Direccion, Cantidad_Pisos)
VALUES
('Torre Central', 'Av. Principal 123', 10),
('Residencial Vista Azul', 'Calle 45, San José', 8),
('Condominio Las Palmas', 'Boulevard de la Paz 456', 12);

-- 2. Insertar Apartamentos
INSERT INTO Apartamento (Codigo_Apartamento, ID_Edificio, Piso, Metros_Cuadrados, Cantidad_Habitantes, Cant_Sanitarios, Disponible)
VALUES
('A101', 1, 1, 80.5, 3, 2, 1),
('A502', 1, 5, 65.0, 2, 1, 1),
('B201', 2, 2, 90.0, 4, 2, 0),
('C1003', 3, 10, 120.0, 5, 3, 1);

-- 3. Insertar Clientes
INSERT INTO Cliente (Nombre, Apellido, Cedula, Telefono, Correo, Fecha_Nacimiento)
VALUES
('Juan', 'Pérez', '1-1234-5678', '8888-1111', 'juan.perez@email.com', '1985-06-15'),
('María', 'Gómez', '2-2345-6789', '8888-2222', 'maria.gomez@email.com', '1990-03-22'),
('Luis', 'Ramírez', '3-3456-7890', '8888-3333', 'luis.ramirez@email.com', '1978-11-05');

-- 4. Insertar Avisos
INSERT INTO Aviso (Titulo, Contenido, Fecha_Publicacion, Autor)
VALUES
('Mantenimiento de ascensor', 'El ascensor estará fuera de servicio el próximo sábado por mantenimiento.', '2025-08-05', 'Administrador'),
('Fiesta de la comunidad', 'Se invita a todos los residentes a la fiesta anual en el área recreativa.', '2025-08-08', 'Comité de Vecinos');

-- 5. Relación Aviso-Cliente
INSERT INTO Aviso_Cliente (ID_Cliente, ID_Aviso)
VALUES
(1, 1),
(2, 1),
(3, 2);

-- 6. Insertar Contratos
INSERT INTO Contrato (ID_Cliente, ID_Apartamento, Fecha_Inicio, Fecha_Fin, Monto_Mensual, Estado)
VALUES
(1, 1, '2025-01-01', '2025-12-31', 750.00, 'Activo'),
(2, 2, '2025-02-01', '2025-11-30', 600.00, 'Activo'),
(3, 3, '2024-05-01', '2025-04-30', 900.00, 'Finalizado');

-- 7. Insertar Pagos
INSERT INTO Pago (ID_Contrato, Fecha_Pago, Monto_Pago, Metodo_Pago)
VALUES
(1, '2025-08-01', 750.00, 'Transferencia'),
(2, '2025-08-03', 600.00, 'Efectivo'),
(3, '2025-04-30', 900.00, 'Tarjeta');

-- 8. Insertar Vehículos
INSERT INTO Vehiculo (ID_Cliente, Placa, Marca, Modelo, Color)
VALUES
(1, 'ABC123', 'Toyota', 'Corolla', 'Rojo'),
(2, 'XYZ789', 'Honda', 'Civic', 'Negro'),
(3, 'LMN456', 'Nissan', 'Versa', 'Blanco');

-- 9. Insertar Mantenimientos
INSERT INTO Mantenimiento (ID_Apartamento, Descripcion, Fecha_Mantenimiento, Costo, Tipo)
VALUES
(1, 'Cambio de tuberías del baño', '2025-07-20', 150.00, 'Plomería'),
(2, 'Pintura de paredes', '2025-07-25', 200.00, 'Pintura'),
(3, 'Revisión de instalación eléctrica', '2025-08-01', 120.00, 'Electricidad');

-- 10. Insertar Áreas Recreativas
INSERT INTO AreaRecreativa (Nombre, Descripcion, Horario)
VALUES
('Piscina', 'Piscina semi-olímpica para uso de residentes', '06:00 - 20:00'),
('Gimnasio', 'Equipado con máquinas modernas y área de pesas', '05:00 - 22:00');

-- 11. Insertar Roles
INSERT INTO Roll (Descripcion)
VALUES
('Administrador'),
('Usuario');




----PROCEDIMIENTOS--------------------------------------------------------------------




--CRUD PARA TABLA EDIFICIO

CREATE PROCEDURE sp_InsertEdificio
    @Nombre VARCHAR(100),
    @Direccion VARCHAR(150),
    @Cantidad_Pisos INT
AS
BEGIN
    INSERT INTO Edificio (Nombre, Direccion, Cantidad_Pisos)
    VALUES (@Nombre, @Direccion, @Cantidad_Pisos);
END
GO

CREATE PROCEDURE sp_GetEdificios
AS
BEGIN
    SELECT * FROM Edificio;
END
GO

CREATE PROCEDURE sp_UpdateEdificio
    @ID_Edificio INT,
    @Nombre VARCHAR(100),
    @Direccion VARCHAR(150),
    @Cantidad_Pisos INT
AS
BEGIN
    UPDATE Edificio
    SET Nombre = @Nombre,
        Direccion = @Direccion,
        Cantidad_Pisos = @Cantidad_Pisos
    WHERE ID_Edificio = @ID_Edificio;
END
GO

CREATE PROCEDURE sp_DeleteEdificio
    @ID_Edificio INT
AS
BEGIN
    DELETE FROM Edificio
    WHERE ID_Edificio = @ID_Edificio;
END
GO



--CRUD PARA TABLA APARTAMENTO

CREATE PROCEDURE sp_InsertApartamento
    @Codigo_Apartamento VARCHAR(8),
    @ID_Edificio INT,
    @Piso INT,
    @Metros_Cuadrados FLOAT,
    @Cantidad_Habitantes INT,
    @Cant_Sanitarios INT,
    @Disponible BIT
AS
BEGIN
    INSERT INTO Apartamento (Codigo_Apartamento, ID_Edificio, Piso, Metros_Cuadrados, Cantidad_Habitantes, Cant_Sanitarios, Disponible)
    VALUES (@Codigo_Apartamento, @ID_Edificio, @Piso, @Metros_Cuadrados, @Cantidad_Habitantes, @Cant_Sanitarios, @Disponible);
END
GO

CREATE PROCEDURE sp_GetApartamentos
AS
BEGIN
    SELECT * FROM Apartamento;
END
GO

CREATE PROCEDURE sp_UpdateApartamento
    @ID_Apartamento INT,
    @Codigo_Apartamento VARCHAR(8),
    @ID_Edificio INT,
    @Piso INT,
    @Metros_Cuadrados FLOAT,
    @Cantidad_Habitantes INT,
    @Cant_Sanitarios INT,
    @Disponible BIT
AS
BEGIN
    UPDATE Apartamento
    SET Codigo_Apartamento = @Codigo_Apartamento,
        ID_Edificio = @ID_Edificio,
        Piso = @Piso,
        Metros_Cuadrados = @Metros_Cuadrados,
        Cantidad_Habitantes = @Cantidad_Habitantes,
        Cant_Sanitarios = @Cant_Sanitarios,
        Disponible = @Disponible
    WHERE ID_Apartamento = @ID_Apartamento;
END
GO

CREATE PROCEDURE sp_DeleteApartamento
    @ID_Apartamento INT
AS
BEGIN
    DELETE FROM Apartamento
    WHERE ID_Apartamento = @ID_Apartamento;
END
GO



--CRUD PARA TABLA CLIENTE

CREATE PROCEDURE sp_InsertCliente
    @Nombre VARCHAR(30),
    @Apellido VARCHAR(60),
    @Cedula VARCHAR(20),
    @Telefono VARCHAR(12),
    @Correo VARCHAR(50),
    @Fecha_Nacimiento DATE
AS
BEGIN
    INSERT INTO Cliente (Nombre, Apellido, Cedula, Telefono, Correo, Fecha_Nacimiento)
    VALUES (@Nombre, @Apellido, @Cedula, @Telefono, @Correo, @Fecha_Nacimiento);
END
GO

CREATE PROCEDURE sp_GetClientes
AS
BEGIN
    SELECT * FROM Cliente;
END
GO

CREATE PROCEDURE sp_UpdateCliente
    @ID_Cliente INT,
    @Nombre VARCHAR(30),
    @Apellido VARCHAR(60),
    @Cedula VARCHAR(20),
    @Telefono VARCHAR(12),
    @Correo VARCHAR(50),
    @Fecha_Nacimiento DATE
AS
BEGIN
    UPDATE Cliente
    SET Nombre = @Nombre,
        Apellido = @Apellido,
        Cedula = @Cedula,
        Telefono = @Telefono,
        Correo = @Correo,
        Fecha_Nacimiento = @Fecha_Nacimiento
    WHERE ID_Cliente = @ID_Cliente;
END
GO

CREATE PROCEDURE sp_DeleteCliente
    @ID_Cliente INT
AS
BEGIN
    DELETE FROM Cliente
    WHERE ID_Cliente = @ID_Cliente;
END
GO



--CRUD PARA TABLA CONTRATO

CREATE PROCEDURE sp_InsertContrato
    @ID_Cliente INT,
    @ID_Apartamento INT,
    @Fecha_Inicio DATE,
    @Fecha_Fin DATE,
    @Monto_Mensual FLOAT,
    @Estado VARCHAR(20)
AS
BEGIN
    INSERT INTO Contrato (ID_Cliente, ID_Apartamento, Fecha_Inicio, Fecha_Fin, Monto_Mensual, Estado)
    VALUES (@ID_Cliente, @ID_Apartamento, @Fecha_Inicio, @Fecha_Fin, @Monto_Mensual, @Estado);
END
GO

CREATE PROCEDURE sp_GetContratos
AS
BEGIN
    SELECT * FROM Contrato;
END
GO

CREATE PROCEDURE sp_UpdateContrato
    @ID_Contrato INT,
    @ID_Cliente INT,
    @ID_Apartamento INT,
    @Fecha_Inicio DATE,
    @Fecha_Fin DATE,
    @Monto_Mensual FLOAT,
    @Estado VARCHAR(20)
AS
BEGIN
    UPDATE Contrato
    SET ID_Cliente = @ID_Cliente,
        ID_Apartamento = @ID_Apartamento,
        Fecha_Inicio = @Fecha_Inicio,
        Fecha_Fin = @Fecha_Fin,
        Monto_Mensual = @Monto_Mensual,
        Estado = @Estado
    WHERE ID_Contrato = @ID_Contrato;
END
GO

CREATE PROCEDURE sp_DeleteContrato
    @ID_Contrato INT
AS
BEGIN
    DELETE FROM Contrato
    WHERE ID_Contrato = @ID_Contrato;
END
GO



--CRUD PARA TABLA PAGO

CREATE PROCEDURE sp_InsertPago
    @ID_Contrato INT,
    @Fecha_Pago DATE,
    @Monto_Pago FLOAT,
    @Metodo_Pago VARCHAR(20)
AS
BEGIN
    INSERT INTO Pago (ID_Contrato, Fecha_Pago, Monto_Pago, Metodo_Pago)
    VALUES (@ID_Contrato, @Fecha_Pago, @Monto_Pago, @Metodo_Pago);
END
GO

CREATE PROCEDURE sp_GetPagos
AS
BEGIN
    SELECT * FROM Pago;
END
GO

CREATE PROCEDURE sp_UpdatePago
    @ID_Pago INT,
    @ID_Contrato INT,
    @Fecha_Pago DATE,
    @Monto_Pago FLOAT,
    @Metodo_Pago VARCHAR(20)
AS
BEGIN
    UPDATE Pago
    SET ID_Contrato = @ID_Contrato,
        Fecha_Pago = @Fecha_Pago,
        Monto_Pago = @Monto_Pago,
        Metodo_Pago = @Metodo_Pago
    WHERE ID_Pago = @ID_Pago;
END
GO

CREATE PROCEDURE sp_DeletePago
    @ID_Pago INT
AS
BEGIN
    DELETE FROM Pago
    WHERE ID_Pago = @ID_Pago;
END
GO
