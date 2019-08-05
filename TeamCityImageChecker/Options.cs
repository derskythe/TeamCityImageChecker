using CommandLine;

namespace TeamCityImageChecker
{
    public class Options
    {
        [Option('i', "image", Required = true, HelpText = "Set image name.")]
        public string Image { get; set; }

        [Option('r', "registry", Required = true, HelpText = "Set Docker RegistryV2 URL.")]
        public string Registry { get; set; }
    }
}
