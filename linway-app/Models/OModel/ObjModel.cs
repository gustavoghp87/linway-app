namespace Models.OModel
{
    abstract public class ObjModel
    {
        public long Id { get; set; }
    }
    abstract public class ObjModelConEstado : ObjModel
    {
        public string Estado { get; set; }
    }
}

