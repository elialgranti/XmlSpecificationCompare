$NUGET = 'nuget'
if (-Not (Get-Command $NUGET))
{
    Write-Output "Cannot find nuget.exe. Make sure nuget.exe is in a directory in the PATH"
    Return
}

# Main Nuget package
Invoke-Expression "$NUGET pack XmlSpecificationCompare\XmlSpecificationCompare.csproj -Build -Symbols -Prop Configuration=Release -OutputDirectory Nuget"

# XmlSpecificationEquality code package

$xmlBase = 'Nuget\CodeXml'
$xmlRoot = 'Nuget\CodeXml\content\net45\XmlSpecificationCompare'

if (-Not (Test-Path $xmlRoot -PathType Container))
{
    New-Item $xmlRoot -ItemType directory
}

Remove-Item $xmlRoot\*

(Get-Content XmlSpecificationCompare\XmlSpecificationEquality.cs) | 
Foreach-Object {$_ -replace 'namespace XmlSpecificationCompare', 'namespace $rootnamespace$.XmlSpecificationCompare'} | 
Set-Content $xmlRoot\XmlSpecificationEquality.cs.pp

(Get-Content XmlSpecificationCompare\XmlEqualityResult.cs) | 
Foreach-Object {$_ -replace 'namespace XmlSpecificationCompare', 'namespace $rootnamespace$.XmlSpecificationCompare'} | 
Set-Content $xmlRoot\XmlEqualityResult.cs.pp

Invoke-Expression "$NUGET pack XmlSpecificationEquality.Code.nuspec -BasePath $xmlBase -OutputDirectory Nuget"

# XPathDiscovery code package

$xpathBase = 'Nuget\CodeXPath'
$xpathRoot = 'Nuget\CodeXPath\content\net45\XPathDiscovery'
if (-Not (Test-Path $xpathRoot -PathType Container))
{
    New-Item $xpathRoot -ItemType directory
}

Remove-Item $xpathRoot\*


(Get-Content XmlSpecificationCompare\XPathDiscovery\AttributeXPathName.cs) | 
Foreach-Object {$_ -replace 'namespace XmlSpecificationCompare.XPathDiscovery', 'namespace $rootnamespace$.XPathDiscovery'} | 
Set-Content $xpathRoot\AttributeXPathName.cs.pp

(Get-Content XmlSpecificationCompare\XPathDiscovery\CommentXPathName.cs) | 
Foreach-Object {$_ -replace 'namespace XmlSpecificationCompare.XPathDiscovery', 'namespace $rootnamespace$.XPathDiscovery'} | 
Set-Content $xpathRoot\CommentXPathName.cs.pp

(Get-Content XmlSpecificationCompare\XPathDiscovery\ElementXPathName.cs) | 
Foreach-Object {$_ -replace 'namespace XmlSpecificationCompare.XPathDiscovery', 'namespace $rootnamespace$.XPathDiscovery'} | 
Set-Content $xpathRoot\ElementXPathName.cs.pp

(Get-Content XmlSpecificationCompare\XPathDiscovery\IObjectXpathName.cs) | 
Foreach-Object {$_ -replace 'namespace XmlSpecificationCompare.XPathDiscovery', 'namespace $rootnamespace$.XPathDiscovery'} | 
Set-Content $xpathRoot\IObjectXpathName.cs.pp

(Get-Content XmlSpecificationCompare\XPathDiscovery\TextXPathName.cs) | 
Foreach-Object {$_ -replace 'namespace XmlSpecificationCompare.XPathDiscovery', 'namespace $rootnamespace$.XPathDiscovery'} | 
Set-Content $xpathRoot\TextXPathName.cs.pp

(Get-Content XmlSpecificationCompare\XPathDiscovery\XpathExtension.cs) | 
Foreach-Object {$_ -replace 'namespace XmlSpecificationCompare.XPathDiscovery', 'namespace $rootnamespace$.XPathDiscovery'} | 
Set-Content $xpathRoot\XpathExtension.cs.pp

Invoke-Expression "$NUGET pack XPathDiscovery.Code.nuspec -BasePath $xpathBase -OutputDirectory Nuget"