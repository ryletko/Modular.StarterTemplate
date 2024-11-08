namespace Modular.Framework.Domain.BusinessRules
{
    public class BusinessRuleValidationException(BusinessRule brokenRule) : Exception(brokenRule.ErrorMessage)
    {
        public BusinessRule BrokenRule { get; } = brokenRule;

        public string Details { get; } = brokenRule.ErrorMessage;

        public override string ToString()
        {
            return $"{BrokenRule.GetType().FullName}: {BrokenRule.ErrorMessage}";
        }
    }
}
