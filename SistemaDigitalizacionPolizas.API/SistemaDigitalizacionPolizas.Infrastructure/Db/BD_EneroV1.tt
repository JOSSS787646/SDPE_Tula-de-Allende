
USE SDPE_dev;
GO

-- =============================================
-- Tabla: UnidadesAdministrativas
-- =============================================
CREATE TABLE UnidadesAdministrativas (
    idUnidadesAdministrativas INT IDENTITY(1,1) PRIMARY KEY,
    clave INT,
    descripcion VARCHAR(60)
);
GO

-- =============================================
-- Tabla: Roles
-- =============================================
CREATE TABLE Roles (
    idRoles INT IDENTITY(1,1) PRIMARY KEY,
    nombreRol VARCHAR(45) NOT NULL,
    descripcion VARCHAR(100),
    activo BIT NOT NULL DEFAULT 1
);
GO

-- =============================================
-- Tabla: Permisos
-- =============================================
CREATE TABLE Permisos (
    idPermiso INT IDENTITY(1,1) PRIMARY KEY,
    nombrePermiso VARCHAR(250) NULL,
    modulo VARCHAR(250) NULL,
    accion VARCHAR(250) NULL,
    descripcion VARCHAR(250)
);
GO

-- =============================================
-- Tabla: Usuarios
-- =============================================
CREATE TABLE Usuarios (
    idUsuario INT IDENTITY(1,1) PRIMARY KEY,
    email VARCHAR(60) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    idUnidadAdministrativa INT NOT NULL,
    activo BIT NOT NULL DEFAULT 1,
    ultimoAcceso DATETIME NULL,
    createdAt DATETIME NOT NULL DEFAULT GETDATE(),
    updateAt DATETIME NULL,
    idRoles INT NOT NULL
);
GO

-- =============================================
-- Tabla: RolesPermiso (relación N:M)
-- =============================================
CREATE TABLE RolesPermiso (
    idRolPermiso INT IDENTITY(1,1) PRIMARY KEY,
    idRoles INT NOT NULL,
    idPermiso INT NOT NULL
);
GO

-- =============================================
-- Relaciones (FOREIGN KEYS)
-- =============================================

-- Usuarios -> UnidadesAdministrativas
ALTER TABLE Usuarios
ADD CONSTRAINT FK_Usuarios_UnidadesAdministrativas
FOREIGN KEY (idUnidadAdministrativa)
REFERENCES UnidadesAdministrativas(idUnidadesAdministrativas);
GO

-- Usuarios -> Roles
ALTER TABLE Usuarios
ADD CONSTRAINT FK_Usuarios_Roles
FOREIGN KEY (idRoles)
REFERENCES Roles(idRoles);
GO

-- RolesPermiso -> Roles
ALTER TABLE RolesPermiso
ADD CONSTRAINT FK_RolesPermiso_Roles
FOREIGN KEY (idRoles)
REFERENCES Roles(idRoles);
GO

-- RolesPermiso -> Permisos
ALTER TABLE RolesPermiso
ADD CONSTRAINT FK_RolesPermiso_Permisos
FOREIGN KEY (idPermiso)
REFERENCES Permisos(idPermiso);
GO



--Actulizacion de la bd en dia 26/01/2026
CREATE TABLE RecuperarContrasenia (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    Codigo VARCHAR(6) NOT NULL,
    FechaExpiracion DATETIME NOT NULL,
    Usado BIT NOT NULL DEFAULT 0,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_RecuperarContrasenia_Usuarios
        FOREIGN KEY (IdUsuario)
        REFERENCES Usuarios(idUsuario)
);



--Tabla de Permisos es un index para agializar las consultas con relacion a los permisos
CREATE NONCLUSTERED INDEX IX_Permisos_Modulo
ON Permisos (modulo);

CREATE NONCLUSTERED INDEX IX_Permisos_Id
ON Permisos (idPermiso);

--Tablde Roles Permisos
CREATE NONCLUSTERED INDEX IX_RolesPermiso_Rol
ON RolesPermiso (idRoles);

CREATE NONCLUSTERED INDEX IX_RolesPermiso_Permiso
ON RolesPermiso (idPermiso);

CREATE UNIQUE NONCLUSTERED INDEX IX_RolesPermiso_Rol_Permiso
ON RolesPermiso (idRoles, idPermiso);

--Tabla de usuarios
CREATE NONCLUSTERED INDEX IX_Usuarios_Email
ON Usuarios (email);

CREATE NONCLUSTERED INDEX IX_Usuarios_Rol
ON Usuarios (idRoles);





--Procedeures--

--Obtener permisos por rol--
CREATE PROCEDURE sp_ObtenerPermisosPorRol
    @idRol INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.idPermiso,
        p.modulo,
        p.accion,
        CAST(
            CASE 
                WHEN rp.idRolPermiso IS NULL THEN 0
                ELSE 1
            END 
        AS BIT) AS asignado
    FROM Permisos p
    LEFT JOIN RolesPermiso rp 
        ON p.idPermiso = rp.idPermiso
        AND rp.idRoles = @idRol
    ORDER BY p.modulo, p.accion;
END;
GO

--Crea un tipo id permiso
CREATE TYPE dbo.PermisoTableType AS TABLE
(
    idPermiso INT
);
GO

--Actuliza permisos por Rol
CREATE PROCEDURE sp_ActualizarPermisosRol
    @idRol INT,
    @Permisos dbo.PermisoTableType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    -- Eliminar permisos actuales
    DELETE FROM RolesPermiso
    WHERE idRoles = @idRol;

    -- Insertar nuevos permisos
    INSERT INTO RolesPermiso (idRoles, idPermiso)
    SELECT @idRol, idPermiso
    FROM @Permisos;

    COMMIT;
END;
GO



--Obtener permisos por usuario

CREATE PROCEDURE sp_ObtenerPermisosPorUsuario
    @idUsuario INT
AS
BEGIN
    SELECT 
        p.modulo,
        p.accion
    FROM Usuarios u
    JOIN RolesPermiso rp ON rp.idRoles = u.idRoles
    JOIN Permisos p ON p.idPermiso = rp.idPermiso
    WHERE u.idUsuario = @idUsuario;
END;
GO


//NUEVOS CAMBIOS A A BD PARA PERMITIRME INGRESAR LOS SIGUIENTES CRUDS QUE SON NECESARIOS
//NO posee relaciones, solo son las tablas

-- =============================================
-- Tabla: ClasificadorObjetoGasto
-- =============================================
CREATE TABLE ClasificadorObjetoGasto (
    idCog INT IDENTITY(1,1) PRIMARY KEY,
    clave INT NOT NULL,
    descripcion VARCHAR(45) NOT NULL
);
GO

-- =============================================
-- Tabla: Fondo
-- =============================================
CREATE TABLE Fondo (
    idFondo INT IDENTITY(1,1) PRIMARY KEY,
    clave INT NOT NULL,
    descripcion VARCHAR(45) NOT NULL
);
GO

-- =============================================
-- Tabla: Proyecto
-- =============================================
CREATE TABLE Proyecto (
    idProyecto INT IDENTITY(1,1) PRIMARY KEY,
    clave INT NOT NULL,
    descripcion VARCHAR(45) NOT NULL
);
GO

-- =============================================
-- Tabla: Prog
-- =============================================
CREATE TABLE Prog (
    idProg INT IDENTITY(1,1) PRIMARY KEY,
    clave INT NOT NULL,
    descripcion VARCHAR(45) NOT NULL
);
GO
