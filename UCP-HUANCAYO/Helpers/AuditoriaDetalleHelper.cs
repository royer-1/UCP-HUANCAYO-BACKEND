using System.Reflection;

namespace UCP_HUANCAYO.Helpers
{
    public static class AuditoriaDetalleHelper
    {
        public static string GenerarDetalle(object obj, string accion)
        {
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var detalles = props.Select(p =>
            {
                var valor = p.GetValue(obj)?.ToString() ?? "null";
                return $"{p.Name}={valor}";
            });
            return $"{accion}: " + string.Join(", ", detalles);
        }

        public static string GenerarCambios(object original, object modificado, string accion)
        {
            var props = original.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var cambios = new List<string>();

            foreach (var p in props)
            {
                var valorOriginal = original.GetType().GetProperty(p.Name)?.GetValue(original)?.ToString() ?? "null";
                var valorNuevo = modificado.GetType().GetProperty(p.Name)?.GetValue(modificado)?.ToString() ?? "null";

                if (valorOriginal != valorNuevo)
                {
                    cambios.Add($"{p.Name}: {valorOriginal} → {valorNuevo}");
                }
            }

            return $"{accion}: " + string.Join(", ", cambios);
        }
    }
}
