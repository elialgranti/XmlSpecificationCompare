if (-Not (Get-Command nuget))
{
    Write-Output "Cannot find nuget.exe. Make sure nuget.exe is in a directory in the PATH"
    Return
}

if (-Not (Test-Path Nuget -PathType Container))
{
    New-Item Nuget -ItemType directory
}

if (-Not (Test-Path Nuget\Code -PathType Container))
{
    New-Item Nuget\Code -ItemType directory
}


# Main Nuget package
nuget pack XmlSpecificationCompare\XmlSpecificationCompare.csproj -Build -Symbols -Prop Configuration=Release -OutputDirectory Nuget


# XmlSpecificationEquality code package
if (-Not (Test-Path Nuget\Code\content\net45 -PathType Container))
{
    New-Item Nuget\Code\content\net45 -ItemType directory
}

Remove-Item Nuget\Code\content\net45\*

(Get-Content XmlSpecificationCompare\XmlSpecificationEquality.cs) | 
Foreach-Object {$_ -replace 'namespace XmlSpecificationCompare', 'namespace $rootnamespace$.XmlSpecificationCompare'} | 
Set-Content Nuget\Code\content\net45\XmlSpecificationEquality.cs.pp

nuget pack XmlSpecificationCompare\XmlSpecificationEquality.Code.nuspec -BasePath Nuget\Code -OutputDirectory Nuget

# XPathDiscovery code package
Remove-Item Nuget\Code\content\net45\*

(Get-Content XmlSpecificationCompare\XPathDiscovery\AttributeXPathName.cs) | 
Foreach-Object {$_ -replace 'namespace XmlSpecificationCompare.XPathDiscovery', 'namespace $rootnamespace$.XPathDiscovery'} | 
Set-Content Nuget\Code\content\net45\AttributeXPathName.cs.pp

(Get-Content XmlSpecificationCompare\XPathDiscovery\CommentXPathName.cs) | 
Foreach-Object {$_ -replace 'namespace XmlSpecificationCompare.XPathDiscovery', 'namespace $rootnamespace$.XPathDiscovery'} | 
Set-Content Nuget\Code\content\net45\CommentXPathName.cs.pp

(Get-Content XmlSpecificationCompare\XPathDiscovery\ElementXPathName.cs) | 
Foreach-Object {$_ -replace 'namespace XmlSpecificationCompare.XPathDiscovery', 'namespace $rootnamespace$.XPathDiscovery'} | 
Set-Content Nuget\Code\content\net45\ElementXPathName.cs.pp

(Get-Content XmlSpecificationCompare\XPathDiscovery\IObjectXpathName.cs) | 
Foreach-Object {$_ -replace 'namespace XmlSpecificationCompare.XPathDiscovery', 'namespace $rootnamespace$.XPathDiscovery'} | 
Set-Content Nuget\Code\content\net45\IObjectXpathName.cs.pp

(Get-Content XmlSpecificationCompare\XPathDiscovery\TextXPathName.cs) | 
Foreach-Object {$_ -replace 'namespace XmlSpecificationCompare.XPathDiscovery', 'namespace $rootnamespace$.XPathDiscovery'} | 
Set-Content Nuget\Code\content\net45\TextXPathName.cs.pp

(Get-Content XmlSpecificationCompare\XPathDiscovery\XpathExtension.cs) | 
Foreach-Object {$_ -replace 'namespace XmlSpecificationCompare.XPathDiscovery', 'namespace $rootnamespace$.XPathDiscovery'} | 
Set-Content Nuget\Code\content\net45\XpathExtension.cs.pp

nuget pack XmlSpecificationCompare\XPathDiscovery.Code.nuspec -BasePath Nuget\Code -OutputDirectory Nuget

Remove-Item Nuget\Code\content\net45\*