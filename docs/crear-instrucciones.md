# Crear Instrucciones para Agentes

## Rol

Actúa como ingeniero de software.

## Tarea

Crea un conjunto de instrucciones para que agentes de IA entiendan el proyecto.

## Contexto
Explora y lee los archivos del proyecto para reunir contexto.

### Plantilla de Instrucciones
Asegura un archivo corto (<= 100 oraciones) y oraciones breves (<= 100 caracteres).
Sigue esta plantilla y guarda en un archivo markdown `./github/copilot-instructions.md`:

````markdown
# Instrucciones para Agentes

## Resumen del Producto
- {Describe brevemente el producto en 2-3 oraciones cortas.}

## Implementación Técnica

### Stack Tecnológico
- Lenguaje: **{language and version}**
- Framework: **{framework and version}**
- Base de datos: **{database}**
- Seguridad: **{security strategy}**
- Tests: **{testing framework }**
- Logging: **{logging tool }**

### Flujo de desarrollo
```bash
# Configurar el proyecto
# Construir/Compilar el proyecto
# Ejecutar el proyecto
# Ejecutar tests
# Desplegar el proyecto
```

### Estructura de carpetas
```text
.										# Raíz del proyecto
├── .github/copilot-instructions.md		# Archivo con instrucciones para agentes
├── README.md							# Documentación principal
├── {other_files}						# Otros archivos relevantes
└── {other_folders}/					# Otras carpetas relevantes
```

## Entorno
- Las variables y procedimientos del código y la documentación deben estar en Español.
- Priorizar concisión sobre gramática en las respuestas.
- Entorno Windows usando terminal powershell.
- La rama por defecto es `main`.
````

## Pasos a seguir:

1. **Resumen del Producto**: 
  - Resume el producto en 2-3 oraciones breves.
2. **Implementación Técnica**:
  - Stack Tecnológico: Lista las tecnologías principales usadas.
  - Flujo de Desarrollo: Comandos para configurar, construir, ejecutar, testear y desplegar.
  - Estructura de Carpetas: Esboza carpetas y archivos principales.
  - Entorno: Lista detalles relevantes del entorno y copia la sección `Entorno`.
3. **Escribe las Instrucciones**: 
  - Sigue la plantilla y mantén la concisión.

## Checklist de Salida

- [ ] El resultado debe ser un archivo markdown llamado `./github/copilot-instructions.md`.
