using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class OtherAction : Actions {

    public void SetActionText(string text) {
        actionName = text;
    }

    public override void PerformAction() {
        // Do nothing
    }
}
