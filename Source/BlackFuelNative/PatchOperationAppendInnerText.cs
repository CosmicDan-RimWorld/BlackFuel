using System.Xml;
using Verse;

/*
 * Patch operation to append the inner text of an XML node.
 * Useful for adding additional info to existing def descriptions.
 * Value must only have one node - the name doesn't matter, only it's
 * text is taken and appended to the matching xpath node's innertext.
 */
namespace CosmicDanLib
{
    class PatchOperationAppendInnerText : PatchOperationPathed
    {
#pragma warning disable CS0649
        private XmlContainer value;
#pragma warning restore CS0649

        protected override bool ApplyWorker(XmlDocument xml)
        {
            XmlNode node = this.value.node;
            bool result = false;
            foreach (object current in xml.SelectNodes(this.xpath))
            {
                result = true;
                XmlNode originalNode = current as XmlNode;
                XmlNode parentNode = originalNode.ParentNode;
                if (node.ChildNodes.Count != 1)
                    return false; // value should only have one tag to replace

                XmlNode newNode = originalNode.Clone();
                newNode.InnerText += " " + node.ChildNodes[0].InnerText.Trim();
                parentNode.InsertBefore(newNode, originalNode);
                parentNode.RemoveChild(originalNode);
            }
            return result;
        }
    }
}
