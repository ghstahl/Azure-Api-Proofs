using System;

namespace AuthHandler.Models
{
    public class Auth : IComparable
    {
        public string Token { get; set; }
        public string Type { get; set; }
        public int CompareTo(object obj)
        {
            if (Equals(obj))
                return 0;
            return -1;
        }
        public override bool Equals(object obj)
        {
            return ShallowEquals(obj);
        }
        public bool ShallowEquals(object obj)
        {
            var other = obj as Auth;
            if (other == null)
            {
                return false;
            }
            if(other.Token != this.Token || other.Type != this.Type)
            {
                return false;
            }
            return true;
        }
    }
}