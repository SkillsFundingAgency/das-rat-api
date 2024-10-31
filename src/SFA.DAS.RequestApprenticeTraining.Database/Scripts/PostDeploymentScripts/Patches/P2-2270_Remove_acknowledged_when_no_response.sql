IF EXISTS (
    SELECT 1 
    FROM ProviderResponseEmployerRequest 
    WHERE 
        ProviderResponseId IS NULL 
        AND AcknowledgedAt IS NOT NULL 
        AND AcknowledgedBy IS NOT NULL
)
BEGIN
    UPDATE ProviderResponseEmployerRequest
    SET AcknowledgedAt = NULL,
        AcknowledgedBy = NULL
    WHERE 
        ProviderResponseId IS NULL 
        AND AcknowledgedAt IS NOT NULL 
        AND AcknowledgedBy IS NOT NULL;
END
