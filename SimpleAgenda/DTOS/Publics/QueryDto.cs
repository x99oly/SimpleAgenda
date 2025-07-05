/*
The QueryDto class is a data transfer object specifically designed for filtering
and querying appointments within the SimpleAgenda library. Its purpose is to 
centralize all possible filtering options in a single, flexible structure that can 
be passed to service methods (such as GetList or Search) without requiring separate 
parameters for each possible filter.
 */

using SimpleAgenda.Enums;

namespace SimpleAgenda.DTOS.Publics
{
    public class QueryDto
    {
        // AppointmentOutDto
        public int? AppointmentId { get; set; } = null;
        public DateTime? Date { get; set; } = null;
        public DateTime? DateStart { get; set; } = null;  // para range de datas
        public DateTime? DateEnd { get; set; } = null;    // para range de datas

        // EventOutDto
        public int? EventId { get; set; } = null;
        public string? EventTitle { get; set; } = null;
        public string? EventDescription { get; set; } = null;

        // LocationOutDto
        public string? PostalCode { get; set; } = null;
        public string? Street { get; set; } = null;
        public string? Number { get; set; } = null;
        public string? Neighborhood { get; set; } = null;
        public string? City { get; set; } = null;
        public BrazilStatesEnum? State { get; set; } = null;
        public string? Country { get; set; } = null;
        public string? Complement { get; set; } = null;

        // Filtros extras que já havíamos pensado
        public StatusEnum? Status { get; set; } = null;
        public List<StatusEnum>? StatusIn { get; set; } = null;
        public bool? IncludeCancelled { get; set; } = false;

        // Pesquisa texto livre (em título e descrição)
        public string? SearchTerm { get; set; } = null;

        // Paginação
        public int? Skip { get; set; } = null;
        public int? Take { get; set; } = null;

        // Ordenação (exemplo: "Date DESC")
        public string? OrderBy { get; set; } = null;
    }
}
