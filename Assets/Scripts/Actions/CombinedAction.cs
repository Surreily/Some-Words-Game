using System.Collections.Generic;
using System.Linq;

public class CombinedAction : IAction {
    private readonly List<IAction> actions;

    public CombinedAction() {
        actions = new List<IAction>();
    }

    public CombinedAction(params IAction[] actions) {
        this.actions = actions.ToList();
    }

    public void Add(IAction newAction) {
        actions.Add(newAction);
    }

    public void Add(IEnumerable<IAction> newActions) {
        actions.AddRange(newActions);
    }

    public void Add(params IAction[] newActions) {
        actions.AddRange(newActions);
    }

    public void Do() {
        foreach (IAction action in actions) {
            action.Do();
        }
    }

    public void Undo() {
        for (int i = actions.Count - 1; i >= 0; i--) {
            actions[i].Undo();
        }
    }
}
