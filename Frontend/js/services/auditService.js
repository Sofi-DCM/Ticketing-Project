import { CONFIG } from "../configURLs/config.js";
import { baseFetch } from "../tools/baseFetch.js";

export const AuditLogService = {
    async GetAuditLogsPaginated(queryDto){
        const params = new URLSearchParams();

        for (const [key, value] of Object.entries(queryDto)) {
            if (value !== null && value !== undefined && value !== "" && value !== "null") {
                params.append(key, value);
            }
        }

        const urlString = params.toString();
        
        const url = urlString 
            ? `${CONFIG.ROUTES.AUDIT_LOG.GET}?${urlString}` 
            : CONFIG.ROUTES.AUDIT_LOG.GET;  
        console.log(url);
        return await baseFetch(url, 'GET');
    }
}