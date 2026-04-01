using Amazon.S3.Model;

/// <summary>
/// Interfaz que define el servicio de almacenamiento de archivos.
///
/// Permite gestionar la subida, obtención, acceso y eliminación de archivos
/// en un almacenamiento externo (ej. AWS S3).
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Sube un archivo al almacenamiento y retorna la ruta o URL generada.
    /// </summary>
    Task<string> UploadAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        string folder);

    /// <summary>
    /// Elimina un archivo del almacenamiento a partir de su ruta.
    /// </summary>
    Task DeleteAsync(string filePath);

    /// <summary>
    /// Genera una URL temporal (pre-firmada) para acceder a un archivo.
    /// 
    /// La URL expira después del tiempo especificado en minutos.
    /// </summary>
    string GetPresignedUrl(string filePath, int minutes = 60);

    /// <summary>
    /// Obtiene un archivo desde el almacenamiento, incluyendo su contenido
    /// y metadata.
    /// </summary>
    Task<GetObjectResponse> GetFileAsync(string filePath);

    /// <summary>
    /// Elimina un archivo del almacenamiento (alternativa a DeleteAsync).
    /// </summary>
    Task DeleteFileAsync(string filePath);
}