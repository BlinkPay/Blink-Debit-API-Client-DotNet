/*
 * Copyright (c) 2023 BlinkPay
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace BlinkDebitApiClient.Model.V1;

/// <summary>
/// The model for a Westpac account.
/// </summary>
[DataContract(Name = "account")]
public class Account : IEquatable<Account>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Account" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    public Account()
    {
    }

    /// <summary>
    /// The account reference ID from account list. This is required if the account selection information was provided to you on the consents endpoint.
    /// </summary>
    /// <value>The account reference ID from account list. This is required if the account selection information was provided to you on the consents endpoint.</value>
    /// <example>&quot;a879865c-9f99-4435-a120-ef6bbbe46976&quot;</example>
    [DataMember(Name = "account_reference_id", EmitDefaultValue = false)]
    public Guid AccountReferenceId { get; private set; }

    /// <summary>
    /// Returns false as AccountReferenceId should not be serialized given that it's read-only.
    /// </summary>
    /// <returns>false (boolean)</returns>
    public bool ShouldSerializeAccountReferenceId()
    {
        return false;
    }

    /// <summary>
    /// The account number.
    /// </summary>
    /// <value>The account number.</value>
    /// <example>&quot;00-0000-0000000-00&quot;</example>
    [DataMember(Name = "account_number", EmitDefaultValue = false)]
    public string AccountNumber { get; private set; }

    /// <summary>
    /// Returns false as AccountNumber should not be serialized given that it's read-only.
    /// </summary>
    /// <returns>false (boolean)</returns>
    public bool ShouldSerializeAccountNumber()
    {
        return false;
    }

    /// <summary>
    /// The account name.
    /// </summary>
    /// <value>The account name.</value>
    /// <example>&quot;Westpac Everyday&quot;</example>
    [DataMember(Name = "name", EmitDefaultValue = false)]
    public string Name { get; private set; }

    /// <summary>
    /// Returns false as Name should not be serialized given that it's read-only.
    /// </summary>
    /// <returns>false (boolean)</returns>
    public bool ShouldSerializeName()
    {
        return false;
    }

    /// <summary>
    /// The available balance.
    /// </summary>
    /// <value>The available balance.</value>
    /// <example>&quot;1000.00&quot;</example>
    [DataMember(Name = "available_balance", EmitDefaultValue = false)]
    public string AvailableBalance { get; private set; }

    /// <summary>
    /// Returns false as AvailableBalance should not be serialized given that it's read-only.
    /// </summary>
    /// <returns>false (boolean)</returns>
    public bool ShouldSerializeAvailableBalance()
    {
        return false;
    }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class Account {\n");
        sb.Append("  AccountReferenceId: ").Append(AccountReferenceId).Append('\n');
        sb.Append("  AccountNumber: ").Append(AccountNumber).Append('\n');
        sb.Append("  Name: ").Append(Name).Append('\n');
        sb.Append("  AvailableBalance: ").Append(AvailableBalance).Append('\n');
        sb.Append("}\n");
        return sb.ToString();
    }

    /// <summary>
    /// Returns the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public virtual string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    /// <summary>
    /// Returns true if objects are equal
    /// </summary>
    /// <param name="input">Object to be compared</param>
    /// <returns>Boolean</returns>
    public override bool Equals(object input)
    {
        return Equals(input as Account);
    }

    /// <summary>
    /// Returns true if Account instances are equal
    /// </summary>
    /// <param name="input">Instance of Account to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(Account input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                AccountReferenceId == input.AccountReferenceId ||
                (AccountReferenceId != null &&
                 AccountReferenceId.Equals(input.AccountReferenceId))
            ) &&
            (
                AccountNumber == input.AccountNumber ||
                (AccountNumber != null &&
                 AccountNumber.Equals(input.AccountNumber))
            ) &&
            (
                Name == input.Name ||
                (Name != null &&
                 Name.Equals(input.Name))
            ) &&
            (
                AvailableBalance == input.AvailableBalance ||
                (AvailableBalance != null &&
                 AvailableBalance.Equals(input.AvailableBalance))
            );
    }

    /// <summary>
    /// Gets the hash code
    /// </summary>
    /// <returns>Hash code</returns>
    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            var hashCode = 41;
            if (AccountReferenceId != null)
            {
                hashCode = (hashCode * 59) + AccountReferenceId.GetHashCode();
            }

            if (AccountNumber != null)
            {
                hashCode = (hashCode * 59) + AccountNumber.GetHashCode();
            }

            if (Name != null)
            {
                hashCode = (hashCode * 59) + Name.GetHashCode();
            }

            if (AvailableBalance != null)
            {
                hashCode = (hashCode * 59) + AvailableBalance.GetHashCode();
            }

            return hashCode;
        }
    }

    /// <summary>
    /// To validate all properties of the instance
    /// </summary>
    /// <param name="validationContext">Validation context</param>
    /// <returns>Validation Result</returns>
    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        // AccountNumber (string) pattern
        var regexAccountNumber = new Regex(@"^\d{2}-\d{4}-\d{7}-\d{2}$", RegexOptions.CultureInvariant);
        if (false == regexAccountNumber.Match(AccountNumber).Success)
        {
            yield return new ValidationResult(
                "Invalid value for AccountNumber, must match a pattern of " + regexAccountNumber,
                new[] { "AccountNumber" });
        }

        // AvailableBalance (string) pattern
        var regexAvailableBalance = new Regex(@"^\d{1,13}\.\d{1,2}$", RegexOptions.CultureInvariant);
        if (false == regexAvailableBalance.Match(AvailableBalance).Success)
        {
            yield return new ValidationResult(
                "Invalid value for AvailableBalance, must match a pattern of " + regexAvailableBalance,
                new[] { "AvailableBalance" });
        }
    }
}