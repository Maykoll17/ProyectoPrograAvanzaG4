USE [master]
GO

IF DB_ID('SistemaAlquiler') IS NULL
    CREATE DATABASE SistemaAlquiler;
GO

USE SistemaAlquiler;
GO

CREATE TABLE [dbo].[Apartamento](
	[ID_Apartamento] [int] IDENTITY(1,1) NOT NULL,
	[Codigo_Apartamento] [varchar](8) NOT NULL,
	[ID_Edificio] [int] NOT NULL,
	[Piso] [int] NOT NULL,
	[Metros_Cuadrados] [float] NOT NULL,
	[Cantidad_Habitantes] [int] NULL,
	[Cant_Sanitarios] [int] NULL,
	[Disponible] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Apartamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Codigo_Apartamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AreaRecreativa]    Script Date: 8/18/2025 7:15:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AreaRecreativa](
	[ID_Area] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](max) NULL,
	[Horario] [nvarchar](100) NULL,
	[ImageUrl] [varchar](300) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Area] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Aviso]    Script Date: 8/18/2025 7:15:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Aviso](
	[ID_Aviso] [int] IDENTITY(1,1) NOT NULL,
	[Titulo] [varchar](100) NOT NULL,
	[Contenido] [text] NOT NULL,
	[Fecha_Publicacion] [date] NOT NULL,
	[Autor] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Aviso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Aviso_Usuario]    Script Date: 8/18/2025 7:15:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Aviso_Usuario](
	[ID_Usuario] [int] NOT NULL,
	[ID_Aviso] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Usuario] ASC,
	[ID_Aviso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cita]    Script Date: 8/18/2025 7:15:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cita](
	[ID_Cita] [int] IDENTITY(1,1) NOT NULL,
	[ID_Usuario] [int] NOT NULL,
	[ID_Apartamento] [int] NOT NULL,
	[FechaCita] [datetime] NOT NULL,
	[Mensaje] [nvarchar](500) NULL,
	[Estado] [varchar](20) NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Cita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contrato]    Script Date: 8/18/2025 7:15:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contrato](
	[ID_Contrato] [int] IDENTITY(1,1) NOT NULL,
	[ID_Usuario] [int] NOT NULL,
	[ID_Apartamento] [int] NOT NULL,
	[Fecha_Inicio] [date] NOT NULL,
	[Fecha_Fin] [date] NOT NULL,
	[Monto_Mensual] [float] NOT NULL,
	[Estado] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Contrato] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Edificio]    Script Date: 8/18/2025 7:15:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Edificio](
	[ID_Edificio] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Direccion] [varchar](150) NOT NULL,
	[Cantidad_Pisos] [int] NOT NULL,
	[Foto] [varchar](300) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Edificio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FotoApartamento]    Script Date: 8/18/2025 7:15:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FotoApartamento](
	[ID_Foto] [int] IDENTITY(1,1) NOT NULL,
	[ID_Apartamento] [int] NOT NULL,
	[UrlFoto] [varchar](300) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Foto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mantenimiento]    Script Date: 8/18/2025 7:15:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mantenimiento](
	[ID_Mantenimiento] [int] IDENTITY(1,1) NOT NULL,
	[ID_Apartamento] [int] NOT NULL,
	[Descripcion] [text] NOT NULL,
	[Fecha_Mantenimiento] [date] NOT NULL,
	[Costo] [float] NOT NULL,
	[Tipo] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Mantenimiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pago]    Script Date: 8/18/2025 7:15:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pago](
	[ID_Pago] [int] IDENTITY(1,1) NOT NULL,
	[ID_Contrato] [int] NOT NULL,
	[Fecha_Pago] [date] NOT NULL,
	[Monto_Pago] [float] NOT NULL,
	[Metodo_Pago] [varchar](20) NULL,
	[Comprobante_URL] [nvarchar](260) NULL,
	[Estado] [varchar](20) NOT NULL,
	[Numero_SINPE] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Pago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TRol]    Script Date: 8/18/2025 7:15:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TRol](
	[IdRol] [int] IDENTITY(1,1) NOT NULL,
	[DescripcionRol] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 8/18/2025 7:15:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[ID_Usuario] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](60) NOT NULL,
	[Cedula] [varchar](20) NOT NULL,
	[Telefono] [varchar](12) NULL,
	[Contrasenna] [varchar](255) NOT NULL,
	[Correo] [varchar](50) NULL,
	[Fecha_Nacimiento] [date] NULL,
	[IdRol] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Cedula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vehiculo]    Script Date: 8/18/2025 7:15:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE [dbo].[Apartamento] ADD  DEFAULT ((1)) FOR [Disponible]
GO
ALTER TABLE [dbo].[Cita] ADD  CONSTRAINT [DF_Cita_Estado]  DEFAULT ('Pendiente') FOR [Estado]
GO
ALTER TABLE [dbo].[Cita] ADD  CONSTRAINT [DF_Cita_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Contrato] ADD  DEFAULT ('Activo') FOR [Estado]
GO
ALTER TABLE [dbo].[Pago] ADD  DEFAULT ('Pendiente') FOR [Estado]
GO
ALTER TABLE [dbo].[Usuario] ADD  DEFAULT ((2)) FOR [IdRol]
GO
ALTER TABLE [dbo].[Apartamento]  WITH CHECK ADD FOREIGN KEY([ID_Edificio])
REFERENCES [dbo].[Edificio] ([ID_Edificio])
GO
ALTER TABLE [dbo].[Aviso_Usuario]  WITH CHECK ADD FOREIGN KEY([ID_Aviso])
REFERENCES [dbo].[Aviso] ([ID_Aviso])
GO
ALTER TABLE [dbo].[Aviso_Usuario]  WITH CHECK ADD FOREIGN KEY([ID_Usuario])
REFERENCES [dbo].[Usuario] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Cita]  WITH CHECK ADD  CONSTRAINT [FK_Cita_Apartamento] FOREIGN KEY([ID_Apartamento])
REFERENCES [dbo].[Apartamento] ([ID_Apartamento])
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [FK_Cita_Apartamento]
GO
ALTER TABLE [dbo].[Cita]  WITH CHECK ADD  CONSTRAINT [FK_Cita_Usuario] FOREIGN KEY([ID_Usuario])
REFERENCES [dbo].[Usuario] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [FK_Cita_Usuario]
GO
ALTER TABLE [dbo].[Contrato]  WITH CHECK ADD FOREIGN KEY([ID_Apartamento])
REFERENCES [dbo].[Apartamento] ([ID_Apartamento])
GO
ALTER TABLE [dbo].[Contrato]  WITH CHECK ADD FOREIGN KEY([ID_Usuario])
REFERENCES [dbo].[Usuario] ([ID_Usuario])
GO
ALTER TABLE [dbo].[FotoApartamento]  WITH CHECK ADD FOREIGN KEY([ID_Apartamento])
REFERENCES [dbo].[Apartamento] ([ID_Apartamento])
GO
ALTER TABLE [dbo].[Mantenimiento]  WITH CHECK ADD FOREIGN KEY([ID_Apartamento])
REFERENCES [dbo].[Apartamento] ([ID_Apartamento])
GO
ALTER TABLE [dbo].[Pago]  WITH CHECK ADD FOREIGN KEY([ID_Contrato])
REFERENCES [dbo].[Contrato] ([ID_Contrato])
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD FOREIGN KEY([IdRol])
REFERENCES [dbo].[TRol] ([IdRol])
GO

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
INSERT INTO Edificio (Nombre, Direccion, Cantidad_Pisos, Foto)
VALUES
('Torre Central', 'Av. Principal 123', 10, '/Images/Edificios/1.png'),
('Residencial Vista Azul', 'Calle 45, San Jos�', 8, '/Images/Edificios/2.png'),
('Condominio Las Palmas', 'Boulevard de la Paz 456', 12, '/Images/Edificios/3.png');

-- Insertar Apartamentos
INSERT INTO Apartamento (Codigo_Apartamento, ID_Edificio, Piso, Metros_Cuadrados, Cantidad_Habitantes, Cant_Sanitarios, Disponible)
VALUES
('A101', 1, 1, 80.5, 3, 2, 1),
('A502', 1, 5, 65.0, 2, 1, 1),
('B201', 2, 2, 90.0, 4, 2, 0),
('C1003', 3, 10, 120.0, 5, 3, 1);

-- Insertar Fotos
INSERT INTO FotoApartamento (ID_Apartamento, UrlFoto)
VALUES
(1, '/Images/Apartamentos/1_1.png'),
(1, '/Images/Apartamentos/1_2.png'),
(2, '/Images/Apartamentos/2_1.jpg'),
(3, '/Images/Apartamentos/3_1.png'),
(4, '/Images/Apartamentos/4_1.png');



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