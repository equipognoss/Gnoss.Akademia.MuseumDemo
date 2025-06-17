# Gnoss.Akademia.DemoMuseo

## Descripción
Este proyecto es una demostración de un sistema de gestión de museos desarrollado con Gnoss.Akademia. Permite gestionar y visualizar colecciones de arte, exposiciones y recursos museísticos.

## Requisitos Previos
- .NET 6.0 o superior
- SQL Server 2019 o superior
- Visual Studio 2022 (recomendado)
- Node.js 16.x o superior (para el frontend)

## Instalación

1. Clonar el repositorio:
```bash
git clone https://github.com/gnoss/Gnoss.Akademia.DemoMuseo.git
```

2. Restaurar las dependencias de .NET:
```bash
dotnet restore
```

3. Configurar la base de datos:
- Ejecutar los scripts SQL en la carpeta `Database`
- Actualizar la cadena de conexión en `appsettings.json`

4. Ejecutar las migraciones:
```bash
dotnet ef database update
```

## Uso

1. Iniciar el proyecto:
```bash
dotnet run
```

2. Acceder a la aplicación:
- Frontend: http://localhost:3000
- API: http://localhost:5000

## Estructura del Proyecto
```
Gnoss.Akademia.DemoMuseo/
├── src/
│   ├── Gnoss.Akademia.DemoMuseo.API/
│   ├── Gnoss.Akademia.DemoMuseo.Core/
│   └── Gnoss.Akademia.DemoMuseo.Infrastructure/
├── tests/
├── Database/
└── docs/
```

## Características Principales
- Gestión de colecciones de arte
- Administración de exposiciones
- Catálogo digital de obras
- Sistema de búsqueda avanzada
- Gestión de usuarios y permisos

## Contribución
1. Fork el proyecto
2. Crear una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request

## Licencia
Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## Contacto
- Email: soporte@gnoss.com
- Sitio web: https://www.gnoss.com