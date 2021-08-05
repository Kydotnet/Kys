# Changelog

## v1.0.1
### Arreglado
- Corregido un error en CsFunction al llamar funciones con parametros infinitos.
- Permitido cualquier tipo de arreglo en parametros infinitos y no solo dynamic.

### Cambiado
- IContext.RootScope ahora tiene un accesor init.

## v1.0.0
### Agregado
- Agregadas las intefaces en lugar de clases base.
- Ahora existe una implementación de IFunction que almacena una función de Kys.
### Cambiado
- Reference se ajusto a los nuevos cambios por interfaces.
- Se ha cambiado las implementaciones de Scope.
### Removido 
- Se removio el delegado Func.