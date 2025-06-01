namespace BuildingBlocks.Domain.Common
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        protected Entity() { }

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(Entity left, Entity right)
            => left?.Equals(right) ?? right is null;

        public static bool operator !=(Entity left, Entity right)
            => !(left == right);

        public override bool Equals(object? obj)
        {
            if (obj is not Entity other || GetType() != other.GetType())
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Id == other.Id;
        }
    }
}
