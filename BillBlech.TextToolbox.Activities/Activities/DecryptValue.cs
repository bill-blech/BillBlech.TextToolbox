using BillBlech.TextToolbox.Activities.Activities.Encryption;
using BillBlech.TextToolbox.Activities.Properties;
using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;

namespace BillBlech.TextToolbox.Activities
{
    [LocalizedDisplayName(nameof(Resources.DecryptValue_DisplayName))]
    [LocalizedDescription(nameof(Resources.DecryptValue_Description))]
    public class DecryptValue : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.DecryptValue_EncryptedText_DisplayName))]
        [LocalizedDescription(nameof(Resources.DecryptValue_EncryptedText_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> EncryptedText { get; set; }

        [LocalizedDisplayName(nameof(Resources.DecryptValue_PlainText_DisplayName))]
        [LocalizedDescription(nameof(Resources.DecryptValue_PlainText_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> PlainText { get; set; }

        [LocalizedDisplayName(nameof(Resources.DecryptValue_EncryptionAlgorithm_DisplayName))]
        [LocalizedDescription(nameof(Resources.DecryptValue_EncryptionAlgorithm_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public enumEncryptType EncryptionAlgorithm { get; set; }

        public enum enumEncryptType
        {
            SHA256,
            AES,
            HMAC,
            ASCIII
        }


        #endregion


        #region Constructors

        public DecryptValue()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (EncryptedText == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(EncryptedText)));
            if (PlainText == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(PlainText)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var encryptionAlgorithm = EncryptionAlgorithm;
            var encryptedText = EncryptedText.Get(context);

            string plainText = null;

            ///////////////////////////
            // Add execution logic HERE
            string password = "BillBlechBillBlechBillBlechBillBlechBillBlech";

            //Select algorithm
            switch (EncryptionAlgorithm)
            {
                //SHA256
                case enumEncryptType.SHA256:
                    plainText = Cipher.Decrypt(encryptedText, password);
                    break;
                //AES
                case enumEncryptType.AES:
                    plainText = AES.DecryptStringAES(encryptedText, password);
                    break;
                //HMAC
                case enumEncryptType.HMAC:
                    plainText = HMAC.SimpleDecryptWithPassword(encryptedText, password);
                    break;
                //ASCIII
                case enumEncryptType.ASCIII:
                    plainText = ASCIII.Decrypt(plainText);
                    break;

            }

            // Outputs
            return (ctx) =>
            {
                PlainText.Set(ctx, plainText);
            };
        }

        #endregion
    }
}

