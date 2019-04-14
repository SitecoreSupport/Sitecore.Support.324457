using Sitecore.Diagnostics;
using Sitecore.ExperienceForms;
using Sitecore.ExperienceForms.Mvc.Pipelines.RenderForm;
using Sitecore.Mvc.Configuration;
using Sitecore.Mvc.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

namespace Sitecore.Support.ExperienceForms.Mvc.Pipelines.RenderForm
{
  public class SetFormParameters : MvcPipelineProcessor<RenderFormEventArgs>
  {
    private readonly IQueryStringProvider _queryStringProvider;

    public SetFormParameters(IQueryStringProvider queryStringProvider)
    {
      Assert.ArgumentNotNull(queryStringProvider, "queryStringProvider");
      _queryStringProvider = queryStringProvider;
    }

    public override void Process(RenderFormEventArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      args.FormHtmlId = args.HtmlHelper.AttributeEncode(args.HtmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(args.ViewModel.ItemId));
      args.Attributes = new Dictionary<string, object>
    {
      {
        "enctype",
        "multipart/form-data"
      },
      {
        "id",
        args.FormHtmlId
      },
      {
        "data-sc-fxb",
        args.ViewModel.ItemId
      }
    };
      if (!string.IsNullOrEmpty(args.ViewModel.CssClass))
      {
        args.Attributes.Add("class", args.ViewModel.CssClass);
      }
      args.QueryString = new RouteValueDictionary(((IEnumerable<string>)_queryStringProvider.QueryParameters.AllKeys.Where(k=>k!=null)).ToDictionary((Func<string, string>)((string key) => key), (Func<string, object>)((string key) => _queryStringProvider.QueryParameters[key])));
      args.RouteName = MvcSettings.SitecoreRouteName;
    }
  }
}