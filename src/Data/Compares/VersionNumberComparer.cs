namespace Infrastructure.Compares
{
    public class VersionNumberComparer : IComparer<string>
    {
        public int Compare(string? version1, string? version2)
        {
            var v1Numbers = version1.Split('.').Select(int.Parse).ToArray();
            var v2Numbers = version2.Split('.').Select(int.Parse).ToArray();

            for (int i = 0; i < v1Numbers.Length; i++)
            {
                if (v1Numbers[i] != v2Numbers[i])
                    return v1Numbers[i].CompareTo(v2Numbers[i]);
            }

            return 0;
        }
    }
}
