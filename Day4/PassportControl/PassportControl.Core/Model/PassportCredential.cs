using System;
using System.Collections.Generic;
using System.Text;

namespace PassportControl.Core.Model
{
  public class PassportCredential : NorthPoleCredential
  {
    /// <inheritdoc/>
    public new bool Valid
    {
      get
      {
        return
          (
            base.Valid &&
            string.IsNullOrEmpty(cid) == false
          );
      }
    }

    /// <summary>
    /// The credential's issuing country
    /// </summary>
    public string cid
    {
      get; private set;
    }

    public PassportCredential(string credentialString) : base(credentialString)
    {
      var supportedFields = new string[]
      {
        "cid"
      };

      foreach (var field in supportedFields)
      {
        SetPropertyByString(field, FindPropertyValue(field, credentialString));
      }
    }
  }
}
