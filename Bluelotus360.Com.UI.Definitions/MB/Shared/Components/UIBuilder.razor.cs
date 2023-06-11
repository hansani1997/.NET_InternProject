

using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Collections.Generic;
using BL10.CleanArchitecture.Domain;

using System.Threading.Tasks;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.CleanArchitecture.Domain.Entities;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components;
public partial class UIBuilder : ComponentBase, IBLUIOperationHelper
{
    [Parameter]
    public object DataObject { get; set; }
    [Parameter]
    public BLUIElement FormObject { get; set; }

    private IList<Node> uiNodes;

    private BLUIElement HeaderSection1;

    [Parameter]
    public EventCallback OnSectionChanged { get; set; }
    [Parameter]
    public IDictionary<string, EventCallback> InteractionLogics { get; set; }
    [Parameter]
    public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }
    [Parameter]
    public string CssClass { get; set; } = "default-class";



    public BLUIElement LinkedUIObject { get; private set; }
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        ReArrangeElements();
    }


    private void ReArrangeElements()
    {
        if (FormObject!=null)
        {
            var childsHash = FormObject.Children.ToLookup(elem => elem.ParentKey);
            foreach (var child in FormObject.Children)
            {
                child.Children = childsHash[child.ElementKey].ToList();
            }
            BLUIElement form = FormObject.Children.Where(x => x.ElementKey == FormObject.ElementKey).FirstOrDefault();
            if (form != null)
            {
                FormObject = form;

            }
            //HeaderSection1 = FormObject.Children.Where(x => x._internalElementName.Equals("HeaderSection_1")).FirstOrDefault();
            //   HeaderSection1.Children = FormObject.Children.Where(x => x.ParentKey==HeaderSection1.ElementKey).ToList();
        }

    }


    private void OnSectionOK()
    {

    }



    private IList<Node> BuildTreeAndGetRoots(IList<BLUIElement> actualObjects)
    {
        var lookup = new Dictionary<long, Node>();
        var rootNodes = new List<Node>();

        foreach (var item in actualObjects)
        {
            // add us to lookup
            Node ourNode;
            if (lookup.TryGetValue(item.ElementKey, out ourNode))
            {   // was already found as a parent - register the actual object
                ourNode.Source = item;
            }
            else
            {
                ourNode = new Node() { Source = item };
                lookup.Add(item.ElementKey, ourNode);
            }

            // hook into parent
            if (item.ParentKey == item.ElementKey)
            {   // is a root node
                rootNodes.Add(ourNode);
            }
            else
            {   // is a child row - so we have a parent
                Node parentNode;
                if (!lookup.TryGetValue(item.ParentKey, out parentNode))
                {   // unknown parent, construct preliminary parent
                    parentNode = new Node();
                    lookup.Add(item.ParentKey, parentNode);
                }
                parentNode.Children.Add(ourNode);
                ourNode.Parent = parentNode;
            }
        }

        return rootNodes;
    }

    private void OnUiElementChange()
    {
        if (OnSectionChanged.HasDelegate)
        {
            OnSectionChanged.InvokeAsync();
        }
    }

    public void ResetToInitialValue()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateVisibility(bool IsVisible)
    {
        throw new System.NotImplementedException();
    }

    public void ToggleEditable(bool IsEditable)
    {
        throw new System.NotImplementedException();
    }

    public async Task Refresh()
    {
        await Task.CompletedTask;
    }


    public Task FocusComponentAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task SetValue(object value)
    {
        throw new System.NotImplementedException();
    }
}

class Node
{
    public List<Node> Children = new List<Node>();
    public Node Parent { get; set; }
    public BLUIElement Source { get; set; }
}
