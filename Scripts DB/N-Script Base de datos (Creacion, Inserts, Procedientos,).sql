-- Crear base de datos
IF DB_ID('SistemaAlquiler') IS NULL
    CREATE DATABASE SistemaAlquiler;
GO

USE SistemaAlquiler;
GO

-- Tabla: Rol
CREATE TABLE TRol(
    IdRol INT IDENTITY(1,1) PRIMARY KEY,
    DescripcionRol VARCHAR(100) NOT NULL
);

-- Tabla: Usuario 
CREATE TABLE Usuario (
    ID_Usuario INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(60) NOT NULL,
    Cedula VARCHAR(20) NOT NULL UNIQUE,
    Telefono VARCHAR(12),
    Contrasenna VARCHAR(255) NOT NULL,
    Correo VARCHAR(50),
    Fecha_Nacimiento DATE,
    IdRol INT NOT NULL DEFAULT 2,
    FOREIGN KEY (IdRol) REFERENCES TRol(IdRol)
);


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

-- Tabla: Aviso
CREATE TABLE Aviso (
    ID_Aviso INT IDENTITY(1,1) PRIMARY KEY,
    Titulo VARCHAR(100) NOT NULL,
    Contenido TEXT NOT NULL,
    Fecha_Publicacion DATE NOT NULL,
    Autor VARCHAR(100) NOT NULL
);

-- Tabla: Relaci�n Aviso-Usuario
CREATE TABLE Aviso_Usuario (
    ID_Usuario INT NOT NULL,
    ID_Aviso INT NOT NULL,
    PRIMARY KEY (ID_Usuario, ID_Aviso),
    FOREIGN KEY (ID_Usuario) REFERENCES Usuario(ID_Usuario),
    FOREIGN KEY (ID_Aviso) REFERENCES Aviso(ID_Aviso)
);

-- Tabla: Contrato
CREATE TABLE Contrato (
    ID_Contrato INT IDENTITY(1,1) PRIMARY KEY,
    ID_Usuario INT NOT NULL,
    ID_Apartamento INT NOT NULL,
    Fecha_Inicio DATE NOT NULL,
    Fecha_Fin DATE NOT NULL,
    Monto_Mensual FLOAT NOT NULL,
    Estado VARCHAR(20) DEFAULT 'Activo',
    FOREIGN KEY (ID_Usuario) REFERENCES Usuario(ID_Usuario),
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
    ID_Usuario INT NOT NULL,
    Placa VARCHAR(20) NOT NULL UNIQUE,
    Marca VARCHAR(20),
    Modelo VARCHAR(20),
    Color VARCHAR(20),
    FOREIGN KEY (ID_Usuario) REFERENCES Usuario(ID_Usuario)
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

-- Tabla: Area Recreativa
CREATE TABLE AreaRecreativa (
    ID_Area INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(MAX),
    Horario NVARCHAR(100)
);


-- Insertar Roles
INSERT INTO TRol (DescripcionRol)
VALUES
('Administrador'),
('Usuario');

-- Insertar Usuarios
INSERT INTO Usuario (Nombre, Cedula, Telefono, Contrasenna, Correo, Fecha_Nacimiento, IdRol)
VALUES
('Admin Sistema', '0-0000-0000', '8888-0000', 'admin123', 'admin@sistema.com', '1980-01-01', 1),
('Juan P�rez', '1-1234-5678', '8888-1111', 'clave123', 'juan.perez@email.com', '1985-06-15', 2),
('Mar�a G�mez', '2-2345-6789', '8888-2222', 'clave456', 'maria.gomez@email.com', '1990-03-22', 2),
('Luis Ram�rez', '3-3456-7890', '8888-3333', 'clave789', 'luis.ramirez@email.com', '1978-11-05', 2);

-- Insertar Edificios
INSERT INTO Edificio (Nombre, Direccion, Cantidad_Pisos)
VALUES
('Torre Central', 'Av. Principal 123', 10),
('Residencial Vista Azul', 'Calle 45, San Jos�', 8),
('Condominio Las Palmas', 'Boulevard de la Paz 456', 12);

-- Insertar Apartamentos
INSERT INTO Apartamento (Codigo_Apartamento, ID_Edificio, Piso, Metros_Cuadrados, Cantidad_Habitantes, Cant_Sanitarios, Disponible)
VALUES
('A101', 1, 1, 80.5, 3, 2, 1),
('A502', 1, 5, 65.0, 2, 1, 1),
('B201', 2, 2, 90.0, 4, 2, 0),
('C1003', 3, 10, 120.0, 5, 3, 1);

-- Insertar Avisos
INSERT INTO Aviso (Titulo, Contenido, Fecha_Publicacion, Autor)
VALUES
('Mantenimiento de ascensor', 'El ascensor estar� fuera de servicio el pr�ximo s�bado por mantenimiento.', '2025-08-05', 'Administrador'),
('Fiesta de la comunidad', 'Se invita a todos los residentes a la fiesta anual en el �rea recreativa.', '2025-08-08', 'Comit� de Vecinos');

-- Relaci�n Aviso-Usuario
INSERT INTO Aviso_Usuario (ID_Usuario, ID_Aviso)
VALUES
(2, 1),
(3, 1),
(4, 2);

-- Insertar Contratos
INSERT INTO Contrato (ID_Usuario, ID_Apartamento, Fecha_Inicio, Fecha_Fin, Monto_Mensual, Estado)
VALUES
(2, 1, '2025-01-01', '2025-12-31', 750.00, 'Activo'),
(3, 2, '2025-02-01', '2025-11-30', 600.00, 'Activo'),
(4, 3, '2024-05-01', '2025-04-30', 900.00, 'Finalizado');

-- Insertar Pagos
INSERT INTO Pago (ID_Contrato, Fecha_Pago, Monto_Pago, Metodo_Pago)
VALUES
(1, '2025-08-01', 750.00, 'Transferencia'),
(2, '2025-08-03', 600.00, 'Efectivo'),
(3, '2025-04-30', 900.00, 'Tarjeta');

-- Insertar Veh�culos
INSERT INTO Vehiculo (ID_Usuario, Placa, Marca, Modelo, Color)
VALUES
(2, 'ABC123', 'Toyota', 'Corolla', 'Rojo'),
(3, 'XYZ789', 'Honda', 'Civic', 'Negro'),
(4, 'LMN456', 'Nissan', 'Versa', 'Blanco');

-- Insertar Mantenimientos
INSERT INTO Mantenimiento (ID_Apartamento, Descripcion, Fecha_Mantenimiento, Costo, Tipo)
VALUES
(1, 'Cambio de tuber�as del ba�o', '2025-07-20', 150.00, 'Plomer�a'),
(2, 'Pintura de paredes', '2025-07-25', 200.00, 'Pintura'),
(3, 'Revisi�n de instalaci�n el�ctrica', '2025-08-01', 120.00, 'Electricidad');

-- Insertar �reas Recreativas
INSERT INTO AreaRecreativa (Nombre, Descripcion, Horario)
VALUES
('Piscina', 'Piscina semi-ol�mpica para uso de residentes', '06:00 - 20:00'),
('Gimnasio', 'Equipado con m�quinas modernas y �rea de pesas', '05:00 - 22:00');


-- Insertar Rol
CREATE PROCEDURE sp_InsertRol
    @DescripcionRol VARCHAR(100)
AS
BEGIN
    INSERT INTO TRol (DescripcionRol)
    VALUES (@DescripcionRol);
END
GO

-- Obtener todos los Roles
CREATE PROCEDURE sp_GetRoles
AS
BEGIN
    SELECT * FROM TRol;
END
GO

-- Actualizar Rol
CREATE PROCEDURE sp_UpdateRol
    @IdRol INT,
    @DescripcionRol VARCHAR(100)
AS
BEGIN
    UPDATE TRol
    SET DescripcionRol = @DescripcionRol
    WHERE IdRol = @IdRol;
END
GO

-- Eliminar Rol
CREATE PROCEDURE sp_DeleteRol
    @IdRol INT
AS
BEGIN
    DELETE FROM TRol
    WHERE IdRol = @IdRol;
END
GO

-- Insertar Usuario
CREATE PROCEDURE sp_InsertUsuario
    @Nombre VARCHAR(30),
    @Cedula VARCHAR(20),
    @Telefono VARCHAR(12),
    @Contrasenna VARCHAR(255),
    @Correo VARCHAR(50),
    @Fecha_Nacimiento DATE
AS
BEGIN
    INSERT INTO Usuario (Nombre, Cedula, Telefono, Contrasenna, Correo, Fecha_Nacimiento, IdRol)
    VALUES (@Nombre, @Cedula, @Telefono, @Contrasenna, @Correo, @Fecha_Nacimiento, 2);
END
GO

-- Obtener todos los Usuarios
CREATE PROCEDURE sp_GetUsuarios
AS
BEGIN
    SELECT * FROM Usuario;
END
GO

-- Actualizar Usuario
CREATE PROCEDURE sp_UpdateUsuario
    @ID_Usuario INT,
    @Nombre VARCHAR(30),
    @Cedula VARCHAR(20),
    @Telefono VARCHAR(12),
    @Contrasenna VARCHAR(255),
    @Correo VARCHAR(50),
    @Fecha_Nacimiento DATE,
    @IdRol INT
AS
BEGIN
    UPDATE Usuario
    SET Nombre = @Nombre,
        Cedula = @Cedula,
        Telefono = @Telefono,
        Contrasenna = @Contrasenna,
        Correo = @Correo,
        Fecha_Nacimiento = @Fecha_Nacimiento,
        IdRol = @IdRol
    WHERE ID_Usuario = @ID_Usuario;
END
GO

-- Eliminar Usuario
CREATE PROCEDURE sp_DeleteUsuario
    @ID_Usuario INT
AS
BEGIN
    DELETE FROM Usuario
    WHERE ID_Usuario = @ID_Usuario;
END
GO

-- Insertar Edificio
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

-- Obtener todos los Edificios
CREATE PROCEDURE sp_GetEdificios
AS
BEGIN
    SELECT * FROM Edificio;
END
GO

-- Actualizar Edificio
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

-- Eliminar Edificio
CREATE PROCEDURE sp_DeleteEdificio
    @ID_Edificio INT
AS
BEGIN
    DELETE FROM Edificio
    WHERE ID_Edificio = @ID_Edificio;
END
GO

-- Insertar Apartamento
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

-- Obtener todos los Apartamentos
CREATE PROCEDURE sp_GetApartamentos
AS
BEGIN
    SELECT * FROM Apartamento;
END
GO

-- Actualizar Apartamento
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

-- Eliminar Apartamento
CREATE PROCEDURE sp_DeleteApartamento
    @ID_Apartamento INT
AS
BEGIN
    DELETE FROM Apartamento
    WHERE ID_Apartamento = @ID_Apartamento;
END
GO

-- Insertar Contrato
CREATE PROCEDURE sp_InsertContrato
    @ID_Usuario INT,
    @ID_Apartamento INT,
    @Fecha_Inicio DATE,
    @Fecha_Fin DATE,
    @Monto_Mensual FLOAT,
    @Estado VARCHAR(20)
AS
BEGIN
    INSERT INTO Contrato (ID_Usuario, ID_Apartamento, Fecha_Inicio, Fecha_Fin, Monto_Mensual, Estado)
    VALUES (@ID_Usuario, @ID_Apartamento, @Fecha_Inicio, @Fecha_Fin, @Monto_Mensual, @Estado);
END
GO

-- Obtener todos los Contratos
CREATE PROCEDURE sp_GetContratos
AS
BEGIN
    SELECT * FROM Contrato;
END
GO

-- Actualizar Contrato
CREATE PROCEDURE sp_UpdateContrato
    @ID_Contrato INT,
    @ID_Usuario INT,
    @ID_Apartamento INT,
    @Fecha_Inicio DATE,
    @Fecha_Fin DATE,
    @Monto_Mensual FLOAT,
    @Estado VARCHAR(20)
AS
BEGIN
    UPDATE Contrato
    SET ID_Usuario = @ID_Usuario,
        ID_Apartamento = @ID_Apartamento,
        Fecha_Inicio = @Fecha_Inicio,
        Fecha_Fin = @Fecha_Fin,
        Monto_Mensual = @Monto_Mensual,
        Estado = @Estado
    WHERE ID_Contrato = @ID_Contrato;
END
GO

-- Eliminar Contrato
CREATE PROCEDURE sp_DeleteContrato
    @ID_Contrato INT
AS
BEGIN
    DELETE FROM Contrato
    WHERE ID_Contrato = @ID_Contrato;
END
GO

-- Obtener apartamentos disponibles
CREATE PROCEDURE sp_GetApartamentosDisponibles
AS
BEGIN
    SELECT * FROM Apartamento WHERE Disponible = 1;
END
GO

-- Obtener contratos activos
CREATE PROCEDURE sp_GetContratosActivos
AS
BEGIN
    SELECT * FROM Contrato WHERE Estado = 'Activo';
END
GO

-- Registrar pago
CREATE PROCEDURE sp_RegistrarPago
    @ID_Contrato INT,
    @Monto_Pago FLOAT,
    @Metodo_Pago VARCHAR(20)
AS
BEGIN
    INSERT INTO Pago (ID_Contrato, Fecha_Pago, Monto_Pago, Metodo_Pago)
    VALUES (@ID_Contrato, GETDATE(), @Monto_Pago, @Metodo_Pago);
END
GO

-- Obtener pagos por contrato
CREATE PROCEDURE sp_GetPagosPorContrato
    @ID_Contrato INT
AS
BEGIN
    SELECT * FROM Pago WHERE ID_Contrato = @ID_Contrato
    ORDER BY Fecha_Pago DESC;
END
GO

-- Obtener veh�culos de un usuario
CREATE PROCEDURE sp_GetVehiculosPorUsuario
    @ID_Usuario INT
AS
BEGIN
    SELECT * FROM Vehiculo WHERE ID_Usuario = @ID_Usuario;
END
GO

